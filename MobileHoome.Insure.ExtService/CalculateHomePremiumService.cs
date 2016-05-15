using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using MobileHoome.Insure.ExtService.AegisRental;
using MobileHoome.Insure.ExtService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MobileHoome.Insure.ExtService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class CalculateHomePremiumService : ICalculateHomePremiumService
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden

        private Quote quote { get; set; }

        private string CountyNb { get; set; }
        private int TerritoryNumber { get; set; }
        public decimal GetPremiumDetail(MobileHome.Insure.Model.Rental.Quote quote, bool generatePolicy = false)
        {
            decimal premium = 0;

            this.quote = quote;

            //prepare XML file for sending and take the result from other service
            using (var cxt = new mhRentalContext())
            {
                //cxt.Configuration.ProxyCreationEnabled = false;
                var customerInfo = cxt.Customers.FirstOrDefault(c => c.Id == quote.CustomerId);

                var locInfo = cxt.ZipInfo.Where(x => x.Zip == customerInfo.Zip).Select(y => new { CountyNumber = y.CountyNumber, TerritoryNb = y.TerritoryNumber }).FirstOrDefault();
                CountyNb = locInfo.CountyNumber;
                TerritoryNumber = locInfo.TerritoryNb;

                var amountCharged = 0M;
                // As per requirement, we are not sending the proc fee to aegis
                if (quote.InstallmentFee.HasValue)
                    amountCharged = quote.Premium.Value;

                XElement rootEle = new XElement("root",
                                        GetPolicyReturnInfo(generatePolicy, amountCharged),
                                        GetPropertyDealerInfo(string.IsNullOrEmpty(customerInfo.Park.Subproducer) ? ConfigurationManager.AppSettings["MHISubproducerCode"] : customerInfo.Park.Subproducer),
                                        GetPropertyInfo(customerInfo),
                                        new XElement("unitinfo", GetHouseUnitInfo(quote.PersonalProperty))
                                        );

                if (quote.Customer.State.Abbr == "NC")
                {
                    rootEle.Element("unitinfo").Add(new XElement("covinfo",
                                                             GetCoverItemInfo(CoverType.persprop, limit: quote.PersonalProperty),
                                                             GetCoverItemInfo(CoverType.deductible, deductible: quote.Deductible),
                                                             GetCoverItemInfo(CoverType.liability, limit: quote.Liability),
                                                             GetCoverItemInfo(CoverType.medpay, limit: quote.MedPay),
                                                             GetCoverItemInfo(CoverType.thirdpartydesignee)
                                                             ));
                }
                else
                {
                     rootEle.Element("unitinfo").Add(new XElement("covinfo",
                                                             GetCoverItemInfo(CoverType.persprop, limit: quote.PersonalProperty),
                                                             GetCoverItemInfo(CoverType.deductible, deductible: quote.Deductible),
                                                             GetCoverItemInfo(CoverType.lou, limit: quote.LOU),
                                                             GetCoverItemInfo(CoverType.liability, limit: quote.Liability),
                                                             GetCoverItemInfo(CoverType.medpay, limit: quote.MedPay),
                                                             GetCoverItemInfo(CoverType.thirdpartydesignee)
                                                             ));
                }
                
                if (quote.SendLandLord)
                {
                    rootEle.Element("unitinfo").Add(new XElement("addl_exposure",
                                                                   GetAdditionalExposure(customerInfo.Park)));
                }
                else
                {
                    rootEle.Element("unitinfo").Element("covinfo").Elements("cov_item").Last().Remove();
                }

                //Call service and get the result with Premium
                ServiceSoapClient sClient = new ServiceSoapClient(ConfigurationManager.AppSettings["ServiceConfigName"]);
                sClient.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                XmlNode result = null;
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(rootEle.ToString());
                XmlNode xnode = doc.FirstChild;

                if (quote.Customer.State.Abbr == "NC")
                {
                    doc = new XmlDocument();
                    rootEle.Element("unitinfo").Add(new XElement("mh_unit"));
                    
                    doc.LoadXml(rootEle.ToString());
                    xnode = doc.FirstChild;
                    
                    var oldElem = xnode.ChildNodes.Item(3).SelectSingleNode("ho_unit");
                    var newElem = xnode.ChildNodes.Item(3).SelectSingleNode("mh_unit");

                    xnode.ChildNodes.Item(3).ReplaceChild(newElem, oldElem);
                    while (oldElem.ChildNodes.Count != 0)
                    {
                        newElem.AppendChild(oldElem.ChildNodes[0]);
                    }
                    while (oldElem.Attributes.Count != 0)
                    {
                        newElem.Attributes.Append(oldElem.Attributes[0]);
                    }

                    result = sClient.QuotePolicy(ConfigurationManager.AppSettings["PasskeyForAegisService"], xnode, "AGMH", AstecProcessingMode.SubmitOverride);
                }
                else
                 result = sClient.QuotePolicy(ConfigurationManager.AppSettings["PasskeyForAegisService"], xnode, "AGHO", AstecProcessingMode.SubmitOverride);

                if (result != null)
                {
                    var elements = result.SelectSingleNode("rtninfo");
                    if (elements != null && elements["returnc"].InnerText != "001")
                    {
                        quote.Premium = premium = Convert.ToDecimal(elements["premwrit"].InnerText);
                        quote.ProposalNumber = elements["policynbr"].InnerText;
                    }

                }
            }
            return premium;
        }

        private XElement GetAdditionalExposure(Park park)
        {
            var additionalExposure = new ThirdPartyDesignee()
            {
                name = park.ParkName,
                addr1 = park.PhysicalAddress,
                addr2 = park.PhysicalAddress2,
                city = park.PhysicalCity,
                state = park.PhysicalState.Abbr,
                zip = park.PhysicalZip.ToString()
            };
            return Helpers.Extensions.ToXml(additionalExposure);
        }


        #region Private methods for generate XML elements

        private XElement GetPolicyReturnInfo(bool generatePolicy = false, decimal premium = 0)
        {
            var returnInfo = new PolicyReturnInfo()
            {
                returnc = generatePolicy ? "A" : "Q",
                premwrit = generatePolicy ? premium.ToString() : "316",
                policynbr = generatePolicy ? "" : LongBetween(9999999999, 1000000000).ToString(), //"4200001254", //Finding new number
                progmode = generatePolicy ? "A" : "Q",
                effdate = String.Format("{0:MM/dd/yyyy}", quote.EffectiveDate.Value), // "06/14/2015", // quote.EffectiveDate.Value.ToShortDateString(),
                productcde = quote.Customer.State.Abbr == "NC" ? "212T" : "42DT",
                lstate = quote.Customer.State.Abbr,
                polterm = "12"
            };


            return Helpers.Extensions.ToXml(returnInfo);
        }

        private XElement GetPropertyDealerInfo(string agentSubProducerNumber)
        {
            var prd = new PropertyDealerInfo()
            {
                agent = 2001,
                agtsb1 = agentSubProducerNumber,
                biltyp = 3
            };

            return Helpers.Extensions.ToXml(prd);
        }

        private XElement GetPropertyInfo(Customer customer)
        {
            var property = new PropertyInfo()
            {
                ppltyp = "I",
                pplnbr = 1,                     //TODO: update from DB
                firstname = customer.FirstName,
                lastname = customer.LastName,
                middleinit = string.Empty,        //TODO: update from DB
                insaddr1 = customer.Address,
                insaddr2 = string.Empty,          //TODO: update from DB
                inscity = customer.City,
                insstate = customer.State.Abbr,
                inscounty = string.Empty, //string.Empty,           //TODO: update from DB
                insctycode = string.Empty,
                inszip = customer.Zip,
                insdob = string.Empty,  //TODO: update from DB
                insfin = string.Empty,
                insssn = string.Empty,   //TODO: update from DB
                insphone = customer.Phone,
                insoccup = string.Empty,   //TODO: update from DB
                reltoapp = string.Empty  //TODO: update from DB
            };

            return Helpers.Extensions.ToXml(property);
        }

        private XElement GetHouseUnitInfo(decimal? limit = 0)
        {
            //TODO: it should be populate from Database
            var houseUnit = new HouseUnitInfo()
            {
                unitnbr = "1",
                make = "UNKNOWN",
                model = "UNKNOWN",
                modelyear = "2000",
                constyear = "2000",
                consttype = "F",
                serialnbr = "UNKNOWN",
                lngth = "0",
                width = "0",
                fireprotcd = "8",
                familycount = string.Empty,
                locaddr1 = quote.Customer.Address,
                locaddr2 = string.Empty,
                loccity = quote.Customer.Park.PhysicalCity,
                locstate = quote.Customer.Park.PhysicalState.Abbr,
                loccountynb = CountyNb, // quote.Customer.Park.PhysicalCounty,
                locterritory = TerritoryNumber.ToString(),
                loczip = quote.Customer.Park.PhysicalZip.ToString(),
                ratingbase = (limit.HasValue ? limit.Value : 0).ToString(),
                parkcode = string.Empty,
                ftfmhyd = "500",
                milfmfde = "5",
                prefrisk = string.Empty,
                protcode = "P",
                purcpric = "0",
                purcdate = "01/2000",
                tiedown = "Y",
                woodstov = "N",
                fndtncod = string.Empty,


            };

            return Helpers.Extensions.ToXml(houseUnit);
        }

        private XElement GetCoverItemInfo(CoverType coverType, decimal? deductible = 0, decimal? limit = 0)
        {
            var coverItem = new CoverItemInfo(coverType);
            if (quote.Customer.State.Abbr == "NC" && coverType == CoverType.liability)
            {
                coverType = CoverType.liab;
            }

            switch (coverType)
            {
                case CoverType.deductible:
                    coverItem.deductible = deductible.HasValue ? deductible.Value.ToString() : "500";
                    coverItem.written_premium = coverItem.inforce_premium = coverItem.limit = string.Empty;
                    break;
                case CoverType.persprop:
                    coverItem.deductible = string.Empty;
                    coverItem.limit = limit.HasValue ? limit.Value.ToString() : "0";
                    coverItem.written_premium = "235";
                    coverItem.inforce_premium = "235";
                    break;
                case CoverType.lou:
                    coverItem.limit = limit.HasValue ? limit.Value.ToString() : "3000";
                    coverItem.written_premium = coverItem.inforce_premium = coverItem.deductible = string.Empty;
                    break;
                case CoverType.liability:
                case CoverType.liab:
                    coverItem.limit = limit.HasValue ? limit.Value.ToString() : "0";
                    coverItem.written_premium = coverItem.inforce_premium = coverItem.deductible = string.Empty;
                    break;
                case CoverType.medpay:
                    coverItem.limit = limit.HasValue ? limit.Value.ToString() : "500";
                    coverItem.written_premium = coverItem.inforce_premium = coverItem.deductible = string.Empty;
                    break;
                case CoverType.thirdpartydesignee:
                    break;
            }

            return Helpers.Extensions.ToXml(coverItem);
        }


        public long LongBetween(long maxValue, long minValue)
        {
            return (long)Math.Round(random.NextDouble() * (maxValue - minValue - 1)) + minValue;
        }
        #endregion
    }
}
