using System;
using System.Collections.Generic;
using MobileHome.Insure.Model.Appraisal;

namespace MobileHome.Insure.Model
{
    public partial class State : Base.BaseEntity
    {
        public State()
        {
            this.StateFactors = new List<StateFactor>();
        }

        
        public string Abbr { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StateFactor> StateFactors { get; set; }

       
    }
}
