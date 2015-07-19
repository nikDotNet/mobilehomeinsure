using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public class User : Base.BaseEntity
    {
        public User()
        {
            this.Customers = new List<Customer>();
        }

        public string EmailId { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
