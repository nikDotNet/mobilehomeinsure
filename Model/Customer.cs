﻿using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public class Customer : Base.BaseEntity
    {
        public Customer()
        {
            this.Payments = new HashSet<MobileHome.Insure.Model.Payment>();
            this.Quotes = new HashSet<Quote>();
        }


        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> StateId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }

        public virtual State State { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<MobileHome.Insure.Model.Payment> Payments { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }

    }
}
