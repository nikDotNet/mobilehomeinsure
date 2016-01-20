using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.DTO
{
    public class ParkDto
    {
        public int Id { get; set; }

        public string ParkName { get; set; }

        public Nullable<int> SpacesToRent { get; set; }

        public Nullable<int> SpacesToOwn { get; set; }

        //Physical address
        public string PhysicalAddress { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("PhysicalState")]
        public Nullable<int> PhysicalStateId { get; set; }

        public string State { get; set; }

        public int PhysicalZip { get; set; }

        public bool IsActive { get; set; }

        public bool IsOn { get; set; }

        //By Vikas
        public string PhysicalCity { get; set; }

        public int TotalOwnRentals { get; set; }
    }
}
