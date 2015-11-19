using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public partial class Park : Base.BaseEntity
    {
        public Park()
        {
            this.Customers = new HashSet<Customer>();
        }
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

        [NotMapped]
        public string MapPhysicalZip { get; set; }


        public string OfficePhone { get; set; }

        public string OfficeFax { get; set; }

        public string OfficeMail { get; set; }

        public string Website { get; set; }

        public Nullable<int> SpacesToRent { get; set; }

        public Nullable<int> SpacesToOwn { get; set; }

        public string ContactName1 { get; set; }

        public string ContactName2 { get; set; }

        public string Position { get; set; }


        //Mailing address
        public string MailingName { get; set; }

        public string MailingAddress { get; set; }

        public string MailingAddress2 { get; set; }

        public string MailingCity { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("MailingState")]
        public Nullable<int> MailingStateId { get; set; }

        public string MailingCsvState { get; set; }

        public string MailingZip { get; set; }


        //Owner Address details
        public string OwnerPhone { get; set; }

        public string OwnerAddress { get; set; }

        public string OwnerAddress2 { get; set; }

        public string OwnerCity { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("OwnerState")]
        public Nullable<int> OwnerStateId { get; set; }

        public string OwnerCsvState { get; set; }

        public string OwnerZip { get; set; }

        // Navigation properties
        public virtual State PhysicalState { get; set; }

        public virtual State MailingState { get; set; }

        public virtual State OwnerState { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
