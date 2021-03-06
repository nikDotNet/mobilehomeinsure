using System;
using System.Collections.Generic;
using MobileHome.Insure.Model.Appraisal;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model
{
    public partial class State : Base.BaseEntity
    {
        public State()
        {
            this.StateFactors = new List<StateFactor>();
            this.OptionsFactors = new List<OptionsFactor>();
            this.Customers = new List<Customer>();
            this.PhysicalParks = new List<Park>();
            this.MailingParks = new List<Park>();
            this.ParkSites = new HashSet<ParkSite>();
            this.InstallmentStructures = new List<InstallmentStructure>();
        }

        [Display(Name="Abbreviation")]
        [Required(ErrorMessage="Please enter abbreviation")]
        public string Abbr { get; set; }
        
        [Display(Name="State")]
        [Required(ErrorMessage="Please enter the name for state")]
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }

        public virtual ICollection<StateFactor> StateFactors { get; set; }
        public virtual ICollection<OptionsFactor> OptionsFactors { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<InstallmentStructure> InstallmentStructures { get; set; }
        public virtual ICollection<Park> PhysicalParks { get; set; }
        
        public virtual ICollection<Park> MailingParks { get; set; }

        public virtual ICollection<Park> OwnerParks { get; set; }
        public virtual ICollection<ParkSite> ParkSites { get; set; }
    }
}
