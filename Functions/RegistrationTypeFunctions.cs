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
using System.Net.Http;
using System.Web.Http;

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
    public  class RegistrationTypeFunctions
    {
        
        private readonly DataContext _context;
        public RegistrationTypeFunctions(DataContext context)
        {
            _context = context;
        }

        [FunctionName("GetRegistrationTypesByEvent")]
        public  async Task<IActionResult> GetRegistrationTypesByEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/registrationtypes/{eventId}")] HttpRequest req,
            ILogger log, int eventId)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<RegistrationType> registrationTypes = new List<RegistrationType>();  
            try  
            {  
                RegistrationTypeController _controller = new RegistrationTypeController(_context);
                
                registrationTypes = _controller.getByEventId(eventId);
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(registrationTypes.Count > 0)  
            {  
                return new OkObjectResult(registrationTypes);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Registration Types Not Found");  
            }  
        }

        
    }

     

}
