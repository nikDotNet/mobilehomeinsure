using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;

namespace mobilehome.insure.Helper.DataImport
{
    public class ParkCsvImport
    {
        public static List<MobileHome.Insure.Model.Park> ParkImport(Stream file)
        {
            List<MobileHome.Insure.Model.Park> listRec = null;

            //Simply reading and storing in collection
            using (var csv = new CsvReader(new StreamReader(file)))
            {
                SetCSVConfiguration(csv);
                listRec = csv.GetRecords<MobileHome.Insure.Model.Park>().ToList();

                //take five digit for Zip Code
                listRec.ForEach((MobileHome.Insure.Model.Park item) =>
                {
                    if (item.MapPhysicalZip.Length > 5)
                    {
                        item.PhysicalZip = Convert.ToInt32(item.MapPhysicalZip.Substring(0, item.MapPhysicalZip.IndexOf("-")));
                    }
                    else
                        item.PhysicalZip = Convert.ToInt32(item.MapPhysicalZip);
                });
                
                var states = new MobileHome.Insure.Service.Master.MasterServiceFacade().GetStates();
                //for physical state
                foreach (var record in listRec.Where(st => !string.IsNullOrWhiteSpace(st.PhysicalCsvState)))
                {
                    record.PhysicalStateId = GetStateIdByStateAbbr(states, record.PhysicalCsvState);
                }

                //for maining state
                foreach (var record in listRec.Where(st => !string.IsNullOrWhiteSpace(st.MailingCsvState)))
                {
                    record.MailingStateId = GetStateIdByStateAbbr(states, record.MailingCsvState);
                }

                //for Owner state
                foreach (var record in listRec.Where(st => !string.IsNullOrWhiteSpace(st.OwnerCsvState)))
                {
                    record.OwnerStateId = GetStateIdByStateAbbr(states, record.OwnerCsvState);
                }
            }

            return listRec;
        }

        private static int? GetStateIdByStateAbbr(List<MobileHome.Insure.Model.State> states, string stateAbbrName)
        {
            var findState = states.FirstOrDefault(ab => ab.Abbr.Trim().ToLower().Contains(stateAbbrName.Trim().ToLower()));
            if (findState != null)
                return findState.Id;

            return null;
        }

        private static void SetCSVConfiguration(CsvReader csv)
        {
            csv.Configuration.SkipEmptyRecords = true;
            csv.Configuration.TrimFields = true;
            csv.Configuration.TrimHeaders = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.RegisterClassMap<ParkFieldMap>();
            //csv.Configuration.IsHeaderCaseSensitive = false;
            //csv.Configuration.IgnoreHeaderWhiteSpace = true;
        }
    }


    #region CSV file column mapper
    public sealed class ParkFieldMap : CsvClassMap<MobileHome.Insure.Model.Park>
    {
        public ParkFieldMap()
        {
            Map(m => m.ParkName).Name("PARK NAME");

            Map(m => m.PhysicalAddress).Name("PHYSICAL ADDRESS");
            Map(m => m.PhysicalAddress2).Name("PHYSICAL ADDRESS 2");
            Map(m => m.PhysicalCity).Name("PHYSICAL CITY");
            Map(m => m.PhysicalCsvState).Name("PHYSICAL STATE");
            Map(m => m.MapPhysicalZip).Name("PHYSICAL ZIP");
            Map(m => m.PhysicalCounty).Name("PHYSICAL COUNTY");

            Map(m => m.OfficePhone).Name("OFFICE PHONE");
            Map(m => m.OfficeFax).Name("OFFICE FAX");
            Map(m => m.OfficeMail).Name("OFFICE EMAIL");
            Map(m => m.Website).Name("WEBSITE");

            Map(m => m.SpacesToRent).Name("SPACES TO RENT");
            Map(m => m.SpacesToOwn).Name("SPACES TO OWN");
            Map(m => m.ContactName1).Name("CONTACT NAME1");
            Map(m => m.ContactName2).Name("CONTACT NAME2");
            Map(m => m.Position).Name("POSITION");

            //mailing
            Map(m => m.MailingName).Name("MAILING NAME");
            Map(m => m.MailingAddress).Name("MAILING ADDRESS");
            Map(m => m.MailingAddress2).Name("MAILING ADDRESS 2");
            Map(m => m.MailingCity).Name("MAILING CITY");
            Map(m => m.MailingCsvState).Name("MAILING STATE");
            Map(m => m.MailingZip).Name("MAILING ZIP");

            //Owner
            Map(m => m.OwnerPhone).Name("OWNER PHONE");
            Map(m => m.OwnerAddress).Name("OWNER ADDRESS");
            Map(m => m.OwnerAddress2).Name("OWNER ADDRESS 2");
            Map(m => m.OwnerCity).Name("OWNER CITY");
            Map(m => m.OwnerCsvState).Name("OWNER STATE");
            Map(m => m.OwnerZip).Name("OWNER ZIP");

            Map(m => m.IsOn).Name("IsActive");
        }
    }
    #endregion
}