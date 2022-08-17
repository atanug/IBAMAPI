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
            var input = JsonConvert.DeserializeObject<MembershipReq>(requestBody); 
            log.LogError(requestBody);
            int membershipId = 0;
            try  
            {  

                

                Membership membership = new Membership{
                    MemberId=input.MemberId,
                    Amount = input.Amount,
                    PmtTypeId=Convert.ToInt16(input.PaymentType),
                    TransactionDate=System.DateTime.Now,
                    Comments=input.Comments,
                    MembershipTypeId=Convert.ToInt16(input.MembershipType),
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
