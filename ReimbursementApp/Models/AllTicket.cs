using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class AllTicket
    {
        
         public AllTicket(Guid reimbursementID, Guid fk_employeeId, string details, Decimal amount, int status)
        {
            ReimbursementID= reimbursementID;
            FK_EmployeeId= fk_employeeId;
            Details = details;
            Amount = amount;
            Status = status;
             
        }


        public Guid ReimbursementID{ get; set; }
        public Guid FK_EmployeeId { get; set; }
        public string  Details { get; set; }
        public Decimal Amount { get; set; }

        public int Status{ get; set; }

        
    }
}