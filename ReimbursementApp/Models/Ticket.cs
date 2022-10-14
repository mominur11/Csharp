using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Ticket
    {
     

         public Ticket(Guid reimbursementID, Guid fk_employeeId, string details, double amount)
        {
            ReimbursementID= reimbursementID;
            FK_EmployeeId= fk_employeeId;
            Details = details;
            Amount = amount;
            //status = status;
             
        }


        public Guid ReimbursementID{ get; set; }
        public Guid FK_EmployeeId { get; set; }
        public string  Details { get; set; }
        public double Amount { get; set; }

        //public string status{ get; set; }
       

    }


}
