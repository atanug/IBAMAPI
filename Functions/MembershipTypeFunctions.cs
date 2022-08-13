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
    public  class MembershipTypeFunctions
    {
        
        private readonly DataContext _context;
        public MembershipTypeFunctions(DataContext context)
        {
            _context = context;
        }

        [FunctionName("GetMembershipTypes")]
        public  async Task<IActionResult> GetMembershipTypes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "membershiptypes")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<MembershipType> membershiptypelist = new List<MembershipType>();  
            try  
            {  
                MembershipTypeController _controller = new MembershipTypeController(_context);
                
                membershiptypelist = _controller.getMembershipTypes();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(membershiptypelist.Count > 0)  
            {  
                return new OkObjectResult(membershiptypelist);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Membership Types Not Found");  
            }  
        }

        
    }

     

}
