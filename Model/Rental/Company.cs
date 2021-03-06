﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.Rental
{
    public class Company : Base.BaseEntity
    {
        public Company()
        {
            this.Quotes = new HashSet<Quote>();
            this.Programs = new List<Program>();
        }

        public string Name { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }
    }
}
