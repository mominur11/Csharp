using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;


namespace Reimbursement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseReimbursemtController : ControllerBase
    {
        private readonly ReimbursementBusinessLayer _businessLayer;
        public ExpenseReimbursemtController()
        {
            this._businessLayer = new ReimbursementBusinessLayer();

        }
        /// <summary>
        //  Get all the pending requests
        //  </summary>

        [HttpGet("RequestsAsync")]  // get all requests
        [HttpGet("RequestsAsync/{type}")]  // Get all of a type of requst
       // [HttpGet("RequestsAsync/{type}/{id}")] // get all or a specific type and employee
       // [HttpGet("RequestsAsync/{id}")] // get all of a specific employees requests

        public async Task<ActionResult<List<ReimbursementReq>>> RequestsAsync(int type, Guid ? id)
        {
                List<ReimbursementReq> requestList = await this._businessLayer.RequestsAsync(type);
                return Ok(requestList);
        }

        [HttpPut("UpdateRequestAsync")]
        public async Task<ActionResult<UpdateReRequestDto>> updateRequestAsync(ApprovalDto approval)
        {
            if (ModelState.IsValid)
            { 
            // Send the ApprivalDto to business layer
             UpdateReRequestDto approvedRequest = await this._businessLayer.UpdateRequestAsync(approval);
            return Ok( approvedRequest);
              }
             else return Conflict(approval);//StatusCode(StatusCodes.Status409Conflict);
        }

        /*
         [HttpPost("AddEmployeeApplicationAsync")]
        public async Task<ActionResult<ReimbursementApplication>> AddEmployeeApplicationAsync( ReimbursementApplication application)
        {
            if (ModelState.IsValid)
            { 
            // Send the ApprivalDto to business layer
             ReimbursementApplication application1 = await this._businessLayer.AddEmployeeApplicationAsync(application);
            return Ok( application1);
              }
             else return Conflict(application);//StatusCode(StatusCodes.Status409Conflict);
        }

        */

         [HttpPost("RegisterNewAccountAsync")]
        public async Task<ActionResult<ReimbursementApplication>> AddEmployeeApplicationAsync( ReimbursementApplication application)
        {
            if (ModelState.IsValid)
            { 
            // Send the ApprivalDto to business layer
             ReimbursementApplication application1 = await this._businessLayer.AddEmployeeApplicationAsync(application);
            return Ok( application1);
              }
             else return Conflict(application);//StatusCode(StatusCodes.Status409Conflict);
        }


         [HttpPost("TicketAsync")]
        public async Task<ActionResult<Ticket>> TicketAsync( Ticket ticket)
        {
            if (ModelState.IsValid)
            { 
            // Send the ApprivalDto to business layer
             Ticket application1 = await this._businessLayer.TicketAsync(ticket);
            return Ok( application1);
              }
             else return Conflict(ticket);//StatusCode(StatusCodes.Status409Conflict);
        }

        [HttpPost("LoginAsync")]
        public async Task<ActionResult<Login>> LoginAsync( Login login)
        {
            if (ModelState.IsValid)
            { 
            // Send the ApprivalDto to business layer
             Login loginTask = await this._businessLayer.LoginAsync(login);
            return Ok( loginTask);
              }
             else return Conflict(login);//StatusCode(StatusCodes.Status409Conflict);
        }


       [HttpGet("AllTicketAsync")]  // get all requests
       

        public async Task<ActionResult<List<AllTicket>>> AllTicketAsync(int type, Guid ? id)
        {
                List<AllTicket> requestList = await this._businessLayer.AllTicketAsync(type);
                return Ok(requestList);
        }

       
    }
}
