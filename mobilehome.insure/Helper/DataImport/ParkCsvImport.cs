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
            Map(m => m.PhysicalZip).Name("PHYSICAL ZIP");
            Map(m => m.PhysicalCounty).Name("PHYSICAL COUNTRY");

            Map(m => m.OfficePhone).Name("OFFICE PHONE");
            Map(m => m.OfficeFax).Name("OFFICE FAX");
            Map(m => m.OfficeMail).Name("OFFICE EMAIL");
            Map(m => m.Website).Name("WEBSITE");

            Map(m => m.SpacesToRent).Name("SPACES TO RENT");
            Map(m => m.SpacesToOwn).Name("SPACES TO OWN");
            Map(m => m.Contact).Name("CONTACT");
            Map(m => m.Position).Name("POSITION");

            Map(m => m.MailingAddress).Name("MAILING ADDRESS");
            Map(m => m.MailingAddress2).Name("MAILING ADDRESS 2");
            Map(m => m.MailingCity).Name("MAILING CITY");
            Map(m => m.MailingCsvState).Name("MAILING STATE");
            Map(m => m.MailingZip).Name("MAILING ZIP");
        }
    }
    #endregion
}