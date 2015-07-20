using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public partial class Park : Base.BaseEntity
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public Nullable<int> Zip4 { get; set; }
        public string County { get; set; }
        public string Phone { get; set; }
        public Nullable<int> Spaces { get; set; }
        public string ContactName { get; set; }
        public string Position { get; set; }
        public virtual State State { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
