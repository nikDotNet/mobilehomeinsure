using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public partial class Park : Base.BaseEntity
    {
        //public int StateId { get; set; }
        //public string Name { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public int Zip { get; set; }
        //public Nullable<int> Zip4 { get; set; }
        //public string County { get; set; }
        //public string Phone { get; set; }
        //public Nullable<int> Spaces { get; set; }
        //public string ContactName { get; set; }
        //public string Position { get; set; }
        //public virtual State State { get; set; }



        public string ParkName { get; set; }

        //Physical address
        public string PhysicalAddress { get; set; }

        public string PhysicalAddress2 { get; set; }

        public string PhysicalCity { get; set; }

        public string PhysicalCounty { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("PhysicalState")]
        public Nullable<int> PhysicalStateId { get; set; }
        
        public string PhysicalCsvState { get; set; }

        public int PhysicalZip { get; set; }


        public string OfficePhone { get; set; }

        public string OfficeFax { get; set; }

        public string OfficeMail { get; set; }

        public string Website { get; set; }

        public Nullable<int> SpacesToRent { get; set; }

        public Nullable<int> SpacesToOwn { get; set; }

        public string Contact { get; set; }

        public string Position { get; set; }


        //Mailing address
        public string MailingAddress { get; set; }

        public string MailingAddress2 { get; set; }

        public string MailingCity { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("MailingState")]
        public Nullable<int> MailingStateId { get; set; }

        public string MailingCsvState { get; set; }

        public Nullable<int> MailingZip { get; set; }


        public virtual State PhysicalState { get; set; }

        public virtual State MailingState { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
