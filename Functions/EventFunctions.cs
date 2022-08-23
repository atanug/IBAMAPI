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
    public  class EventFunctions
    {
        
        private readonly DataContext _context;
        public EventFunctions(DataContext context)
        {
            _context = context;
        }

        [FunctionName("GetEvents")]
        public  async Task<IActionResult> GetEvents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<Event> events = new List<Event>();  
            try  
            {  
                EventController _controller = new EventController(_context);
                
                events = _controller.getEvents();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(events.Count > 0)  
            {  
                return new OkObjectResult(events);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Events Not Found");  
            }  
        }

        
    }

     

}
