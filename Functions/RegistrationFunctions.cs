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
    public  class RegistrationFunctions
    {
        
        private readonly DataContext _context;
        public RegistrationFunctions(DataContext context)
        {
            _context = context;
        }  
        
        
        
        [FunctionName("GetRegistrationByMember")]
        public  async Task<IActionResult> GetRegistrationByEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "members/registration/{memberId}")] HttpRequest req,
            ILogger log, int memberId)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<Registration> RegistrationList = new List<Registration>();  
            try  
            {  
                RegistrationController _controller = new RegistrationController(_context);
                

                RegistrationList = _controller.GetRegistrationsbyMember(memberId);
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(RegistrationList.Count > 0)  
            {  
                return new OkObjectResult(RegistrationList);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Registration Information Not Found");  
            }  
        }

        [FunctionName("CreateRegistration")]  
        public async Task<IActionResult> CreateRegistration(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registrations")] HttpRequest req, ILogger log)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Registration>(requestBody); 
            log.LogError(requestBody);
            int registrationId = 0;
            try  
            {  

                

                Registration Registration = new Registration{
                    MemberId=Convert.ToInt32(input.MemberId),
                    RegistrationTypeId = Convert.ToInt16(input.RegistrationTypeId),
                    Amount = Convert.ToDecimal(input.Amount),
                    PaymentTypeId = Convert.ToInt16(input.PaymentTypeId),
                    TransactionDate=System.DateTime.Now,
                    NoOfPeople = Convert.ToInt16(input.NoOfPeople),
                    Comments=input.Comments,                    
                    CreatedOn=System.DateTime.Now,
                    UpdatedOn=System.DateTime.Now,
                    IsActive = true};

                RegistrationController _controller = new RegistrationController(_context);
                registrationId = _controller.AddRegistration(Registration);

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"createmember",detail:"Error Creating Registration. Please contact system adminstrator."); 
            }  
            return new OkObjectResult(new { id=registrationId}); ;  
        }  

    }
        

}
