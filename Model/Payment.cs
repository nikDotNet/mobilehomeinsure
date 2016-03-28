using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public class Payment : Base.BaseEntity
    {

        public Nullable<decimal> Amount { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionId { get; set; }
        public string ApprovalCode { get; set; }
        public string ApprovalMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string ModeOfPayment { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> RentalQuoteId { get; set; }

        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Quote Quote { get; set; }
    }
}
