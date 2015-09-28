using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.Rental
{
    public class ParkNotify : Base.BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Zip { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsNotified { get; set; }
    }
}
