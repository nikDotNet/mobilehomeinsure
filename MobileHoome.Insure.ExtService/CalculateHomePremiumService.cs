using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
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

        public decimal GetPremiumDetail(MobileHome.Insure.Model.Rental.Quote quote, bool generatePolicy = false)
        {
            decimal premium = 0;

            //prepare XML file for sending and take the result from other service
            using (var cxt = new mhRentalContext())
            {
                //cxt.Configuration.ProxyCreationEnabled = false;
                var customerInfo = cxt.Customers.FirstOrDefault(c => c.Id == quote.CustomerId);
                var amountCharged = 0M;
              // As per requirement, we are not sending the proc fee to aegis
                if(quote.InstallmentFee.HasValue)
                    amountCharged = quote.PremiumChargedToday.Value + quote.InstallmentFee.Value;

                XElement rootEle = new XElement("root",
                                        GetPolicyReturnInfo(generatePolicy,amountCharged),
                                        GetPropertyDealerInfo(string.IsNullOrEmpty(customerInfo.Park.Subproducer) ? customerInfo.Park.Id.ToString() : customerInfo.Park.Subproducer),
                                        GetPropertyInfo(customerInfo),
                                        new XElement("unitinfo", GetHouseUnitInfo(quote.PersonalProperty))
                                        );

                rootEle.Element("unitinfo").Add(new XElement("covinfo",
                                                        GetCoverItemInfo(CoverType.persprop, limit: quote.PersonalProperty),
                                                        GetCoverItemInfo(CoverType.deductible, deductible: quote.Deductible),
                                                        GetCoverItemInfo(CoverType.lou, limit: quote.LOU),
                                                        GetCoverItemInfo(CoverType.liability, limit: quote.Liability),
                                                        GetCoverItemInfo(CoverType.medpay, limit: quote.MedPay),
                                                        GetCoverItemInfo(CoverType.thirdpartydesignee)
                                                        ));
                rootEle.Element("unitinfo").Add(new XElement("addl_exposure",
                                                               GetAdditionalExposure(customerInfo.Park)));

                //Call service and get the result with Premium
                ServiceSoapClient sClient = new ServiceSoapClient(ConfigurationManager.AppSettings["ServiceConfigName"]);
                sClient.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rootEle.ToString());
                XmlNode xnode = doc.FirstChild;

                XmlNode result = sClient.QuotePolicy(ConfigurationManager.AppSettings["PasskeyForAegisService"], xnode, "AGHO", AstecProcessingMode.SubmitOverride);

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
                state = park.PhysicalState.Name,
                zip = park.PhysicalZip.ToString()
            };
            return Helpers.Extensions.ToXml(additionalExposure);
        }


        #region Private methods for generate XML elements

        private XElement GetPolicyReturnInfo(bool generatePolicy = false, decimal premiumCharged = 0)
        {
            var returnInfo = new PolicyReturnInfo()
            {
                returnc = generatePolicy ? "A" : "Q",
                premwrit = generatePolicy ? premiumCharged.ToString() :"316",
                policynbr = generatePolicy ? "" : LongBetween(9999999999, 1000000000).ToString(), //"4200001254", //Finding new number
                progmode = generatePolicy ? "A": "Q",
                effdate = "06/14/2015",
                productcde = "42DT",
                lstate = "SC",
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
                make = "REDMAN",
                model = "5525-L",
                modelyear = "2005",
                constyear = "1997",
                consttype = "F",
                serialnbr = "8EZT1442X5S038820",
                lngth = "70",
                width = "14",
                fireprotcd = "8",
                familycount = string.Empty,
                locaddr1 = "271-273 REFLECTION LAKES",
                locaddr2 = string.Empty,
                loccity = "EQUINUNK",
                locstate = "SC",
                loccountynb = "64",
                locterritory = "1",
                loczip = "18417",
                ratingbase = (limit.HasValue ? limit.Value : 0).ToString(),
                parkcode = string.Empty,
                ftfmhyd = "500",
                milfmfde = "5",
                prefrisk = string.Empty,
                protcode = "P",
                purcpric = "19000",
                purcdate = "09/2013",
                tiedown = "Y",
                woodstov = "N",
                fndtncod = string.Empty,


            };

            return Helpers.Extensions.ToXml(houseUnit);
        }

        private XElement GetCoverItemInfo(CoverType coverType, decimal? deductible =0, decimal? limit =0)
        {
            var coverItem = new CoverItemInfo(coverType);
            switch (coverType)
            {
                case CoverType.deductible:
                    coverItem.deductible = deductible.HasValue ?  deductible.Value.ToString() : "500";
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
