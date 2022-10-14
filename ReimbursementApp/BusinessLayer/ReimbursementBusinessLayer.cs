using Models;
using RepositoryAccessLayer;



namespace BusinessLayer
{
    public class ReimbursementBusinessLayer
    {
        private readonly ReimbursementRepoLayer _repoLayer;
        public ReimbursementBusinessLayer()
        {
            this._repoLayer = new ReimbursementRepoLayer();

        }
        public async Task<List<ReimbursementReq>> RequestsAsync(int type)
        {
            List<ReimbursementReq> list = await this._repoLayer.RequestsAsync(type);
            return list;
        }

          public async Task<UpdateReRequestDto> UpdateRequestAsync(ApprovalDto approvalDto)
        {
            if(await this._repoLayer.IsManagerAsync(approvalDto.EmployeeID))
            { 
               UpdateReRequestDto approvedRequest = await this._repoLayer.UpdateRequestAsync(approvalDto.ReimbursementID,approvalDto.Status);
            
               return approvedRequest;
           }

           else return null;
      }


     public async Task<Ticket> TicketAsync(Ticket ticket)
        {
            Ticket AddTicket = await this._repoLayer.TicketAsync(ticket);
            return AddTicket;
        }

        public async Task<ReimbursementApplication> AddEmployeeApplicationAsync(ReimbursementApplication application)
        {
            ReimbursementApplication application1 = await this._repoLayer.AddEmployeeApplicationAsync(application);
            return application1;
        }

             public async Task<Login> LoginAsync(Login login)
        {
            Login loginTask = await this._repoLayer.LoginAsync(login);
            return loginTask;
        }


        

          public async Task<List<AllTicket>> AllTicketAsync(int type)
        {
            List<AllTicket> ticketlist = await this._repoLayer.AllTicketAsync(type);
            return ticketlist;
        }

         
    }
}