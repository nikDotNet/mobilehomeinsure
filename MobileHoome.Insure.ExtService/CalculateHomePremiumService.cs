using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHoome.Insure.ExtService.AegisRental;
using MobileHoome.Insure.ExtService.Models;
using System;
using System.Collections.Generic;
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


        public decimal GetPremiumDetail(MobileHome.Insure.Model.Rental.Quote quote)
        {
            decimal premium = 0;

            //prepare XML file for sending and take the result from other service
            using (var cxt = new mhRentalContext())
            {
                //cxt.Configuration.ProxyCreationEnabled = false;
                var customerInfo = cxt.Customers.FirstOrDefault(c => c.Id == quote.CustomerId);

                //XElement rootEle = new XElement("root",
                //    //new XElement("rtninfo", GetPolicyReturnInfo()),
                //                        new XElement("prdinfo", GetPropertyDealerInfo()),
                //                        new XElement("pplinfo", GetPropertyInfo(customerInfo)),
                //                        new XElement("unitinfo",
                //                                    new XElement("ho_unit", GetHouseUnitInfo())),
                //                        new XElement("covinfo",
                //                                    new XElement("cov_item", GetCoverItemInfo(CoverType.Persprop)),
                //                                    new XElement("cov_item", GetCoverItemInfo(CoverType.Deductible)),
                //                                    new XElement("cov_item", GetCoverItemInfo(CoverType.LOU))
                //                        ));

                //XElement rootEle = new XElement("root",
                //    //new XElement("rtninfo", GetPolicyReturnInfo()),
                //                        GetPropertyDealerInfo(),
                //                        GetPropertyInfo(customerInfo),
                //                        new XElement("unitinfo", GetHouseUnitInfo()),
                //                        new XElement("covinfo",
                //                                        GetCoverItemInfo(CoverType.Persprop),
                //                                        GetCoverItemInfo(CoverType.Deductible),
                //                                        GetCoverItemInfo(CoverType.LOU)
                //                        ));


                XElement rootEle = new XElement("root",
                                        GetPolicyReturnInfo(),
                                        GetPropertyDealerInfo(),
                                        GetPropertyInfo(customerInfo),
                                        new XElement("unitinfo", GetHouseUnitInfo()),
                                        new XElement("covinfo",
                                                        GetCoverItemInfo(CoverType.Persprop),
                                                        GetCoverItemInfo(CoverType.Deductible),
                                                        GetCoverItemInfo(CoverType.LOU)
                                        ));

                //Call service and get the result with Premium
                ServiceSoapClient sClient = new ServiceSoapClient();
                sClient.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rootEle.ToString());
                XmlNode xnode = doc.FirstChild;
                XmlNode result = sClient.QuotePolicy("edb8f159-416a-4a2f-8018-61463980b727", xnode, "AGHO", AstecProcessingMode.SubmitOverride);

                if (result != null)
                {
                    var elements = result.SelectSingleNode("rtninfo");
                    if (elements != null && !string.IsNullOrWhiteSpace(elements["returnc"].InnerText) && elements["returnc"].InnerText == "000")
                    {
                        quote.Premium = premium = Convert.ToDecimal(elements["premwrit"].InnerText);
                    }
                }
            }
            return premium;
        }


        #region Private methods for generate XML elements

        private XElement GetPolicyReturnInfo()
        {
            var returnInfo = new PolicyReturnInfo()
            {
                returnc = string.Empty,
                premwrit = "316.00",
                policynbr = LongBetween(9999999999, 1000000000).ToString(), //"4200001254", //Finding new number
                progmode = "A",
                effdate = "06/14/2015",
                productcde = "42DT",
                lstate = "SC",
                polterm = "12"
            };


            return Helpers.Extensions.ToXml(returnInfo);
        }

        private XElement GetPropertyDealerInfo()
        {
            var prd = new PropertyDealerInfo()
            {
                agent = 650000,
                agtsb1 = "650000",
                biltyp = 3
            };

            return Helpers.Extensions.ToXml(prd);
        }

        private XElement GetPropertyInfo(Customer customer)
        {
            var property = new PropertyInfo()
            {
                ppltyp = "1",
                pplnbr = 1,                     //TODO: update from DB
                firstname = customer.FirstName,
                lastname = customer.LastName,
                middleinit = string.Empty,        //TODO: update from DB
                insaddr1 = customer.Address,
                insaddr2 = string.Empty,          //TODO: update from DB
                inscity = customer.City,
                insstate = customer.State.Abbr,
                inscounty = string.Empty,           //TODO: update from DB
                insctycode = "77",
                inszip = customer.Zip,
                insdob = DateTime.Parse("12/03/1966"),  //TODO: update from DB
                insfin = "794",
                insssn = "199999200",   //TODO: update from DB
                insphone = customer.Phone,
                insoccup = "BAKER",   //TODO: update from DB
                reltoapp = string.Empty  //TODO: update from DB
            };

            return Helpers.Extensions.ToXml(property);
        }

        private XElement GetHouseUnitInfo()
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
                ratingbase = "25000",
                parkcode = string.Empty,
                ftfmhyd = "500",
                milfmfde = "5",
                prefrisk = string.Empty,
                protcode = "P",
                purcpric = "19000",
                purcdate = "09/2013",
                tiedown = "Y",
                woodstov = "N",
                fndtncod = string.Empty
            };

            return Helpers.Extensions.ToXml(houseUnit);
        }

        private XElement GetCoverItemInfo(CoverType coverType)
        {
            var coverItem = new CoverItemInfo(coverType);
            switch (coverType)
            {
                case CoverType.Deductible:
                    coverItem.deductible = "500";
                    coverItem.written_premium = coverItem.inforce_premium = coverItem.limit = string.Empty;
                    break;
                case CoverType.Persprop:
                    coverItem.deductible = string.Empty;
                    coverItem.limit = "25000";
                    coverItem.written_premium = "235";
                    coverItem.inforce_premium = "235";
                    break;
                case CoverType.LOU:
                    coverItem.limit = "5000";
                    coverItem.written_premium = coverItem.inforce_premium = coverItem.deductible = string.Empty;
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
