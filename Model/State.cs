using System;
using System.Collections.Generic;
using Model.Appraisal;

namespace Model
{
    public partial class State
    {
        public State()
        {
            this.StateFactors = new List<StateFactor>();
        }

        public int Id { get; set; }
        public string Abbr { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StateFactor> StateFactors { get; set; }
    }
}
