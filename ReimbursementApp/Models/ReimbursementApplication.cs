using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ReimbursementApplication
    {
        public ReimbursementApplication(Guid employeeID, string firstName, string lastName, bool isManager, string email, string password)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            IsManager = isManager;
            Email = email;
            Password = password;
        }

        public Guid EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsManager { get; set; }

        public string Email{ get; set; }
        public string Password { get; set; }

    }
}