using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RenterId { get; set; }
        public int CompanyId { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionId { get; set; }
        public string ApprovalCode { get; set; }
        public string ApprovalMessage { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ProposalNumber { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
    }
}
