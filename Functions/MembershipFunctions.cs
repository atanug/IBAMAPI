using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



using IBAM.API.Models;
using IBAM.API.Data;
using IBAM.API.Controllers;
using IBAM.API.Helper;


namespace IBAM.API.Functions
{
    public  class MembershipFunctions
    {
        
        private readonly DataContext _context;
        public MembershipFunctions(DataContext context)
        {
            _context = context;
        }

          

        [FunctionName("CreateMembership")]  
        public async Task<IActionResult> CreateMembership(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "membership")] HttpRequest req, ILogger log)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Membership>(requestBody); 
            log.LogError(requestBody);
            int membershipId = 0;
            try  
            {  

                

                Membership membership = new Membership{
                MemberId=input.MemberId,
                Amount = input.Amount,
                PaymentTypeId=Convert.ToInt16(input.PaymentTypeId),
                TransactionDate=System.DateTime.Now,
                Comments=input.Comments,
                MembershipTypeId=Convert.ToInt16(input.MembershipTypeId),
                IsActive = true,                
                CreatedOn=System.DateTime.Now,
                UpdatedOn=System.DateTime.Now};

                MembershipController _controller = new MembershipController(_context);
                membershipId = _controller.AddMembership(membership);

                

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"createmembership",detail:"Error Creating Membership record. Please contact system adminstrator."); 
            }  
            return new OkObjectResult(new { id=membershipId}); ;  
        }  
        
        [FunctionName("GetMemberships")]
        public  async Task<IActionResult> GetMemberships(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "memberships/member/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<Membership> MembershipList = new List<Membership>();  
            try  
            {  
                MembershipController _controller = new MembershipController(_context);
                

                foreach (Membership membership in _controller.GetMembershipsbyMember(id))
                {
                    MembershipList.Add(membership);
                }
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(MembershipList.Count > 0)  
            {  
                return new OkObjectResult(MembershipList);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Membership information Not Found");  
            }  
        }

        [FunctionName("UpdateMembership")]  
        public async Task<IActionResult> UpdateMembership(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "memberships/{id}")] HttpRequest req, 
            ILogger log, int id)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Membership>(requestBody); 
            log.LogError(requestBody);

            MembershipController _membershipcontroller = new MembershipController(_context);
            Membership membership = new Membership();
            

            try{

                membership = _membershipcontroller.GetMembershipsbyId(id);

                membership.MemberId=input.MemberId;
                membership.Amount = input.Amount;
                membership.PaymentTypeId=Convert.ToInt16(input.PaymentTypeId);
                membership.TransactionDate=System.DateTime.Now;
                membership.Comments=input.Comments;
                membership.MembershipTypeId=Convert.ToInt16(input.MembershipTypeId);
                membership.IsActive = true;                
                membership.UpdatedOn=System.DateTime.Now;
                    

                _membershipcontroller.UpdateMembership(membership);
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"updatemembership",detail:"Error Updating Membership. Please contact system adminstrator."); 
            } 
            return new OkObjectResult(_membershipcontroller.GetMembershipsbyId(id));
        }

        [FunctionName("GetMembershipById")]
        public  async Task<IActionResult> GetMembershipById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "memberships/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }
            
            Membership membership = new Membership();
            
            try  
            {  
                MembershipController _controller = new MembershipController(_context);

                membership = _controller.GetMembershipsbyId(id);

                if (membership==null){

                
                    return ErrorResponse.NotFound(type: "invalid_membership_id",detail:"Membership Not Found");
                      
                }
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator");
            }  
                return new OkObjectResult(membership);  
              
        }


    }

    public class MembershipReq{

        public MembershipReq(){

        }
        
        
        public int MembershipId {get;set;}
        public int MemberId { get; set; }  
        public decimal Amount{get;set;}
        public string PaymentType { get; set; }
        public string Comments { get; set; }
        public string MembershipType { get; set; }
        
    }

}
