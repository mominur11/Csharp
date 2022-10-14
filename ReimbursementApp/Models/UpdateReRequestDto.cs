using System;


namespace Models

{ 
      public class UpdateReRequestDto
     {
        public UpdateReRequestDto(Guid reimbursementId, string firstName, string lastName, int status)
        {
            ReimbursementId = reimbursementId;
            FirstName = firstName;
            LastName = lastName;
            Status = Status;
             
        }

        public Guid ReimbursementId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Status { get; set; }
      

 
    }



}