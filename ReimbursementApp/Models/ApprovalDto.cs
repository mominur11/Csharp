namespace Models
{
    public class ApprovalDto

    {
        public Guid EmployeeID { get; set; }
        public Guid ReimbursementID { get; set; }
        public int Status { get; set; }
    }
}