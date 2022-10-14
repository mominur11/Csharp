using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReimbursementReq
    {
        public ReimbursementReq(Guid reimbursementID, Guid fK_EmployeeID, string details, decimal amount, int status)
        
        {
            ReimbursementID = reimbursementID;
            FK_EmployeeID = fK_EmployeeID;
            Details = details;
            Amount = amount;
            Status = status;
        }

        public Guid ReimbursementID { get; set; }
        public Guid FK_EmployeeID { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }

        public int Status{ get; set; }

    }
}
