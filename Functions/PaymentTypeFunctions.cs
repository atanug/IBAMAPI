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
    public  class PaymentTypeFunctions
    {
        
        private readonly DataContext _context;
        public PaymentTypeFunctions(DataContext context)
        {
            _context = context;
        }

        [FunctionName("GetPaymentTypes")]
        public  async Task<IActionResult> GetPaymentTypes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "paymenttypes")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<PaymentType> pmttypelist = new List<PaymentType>();  
            try  
            {  
                PaymentTypeController _controller = new PaymentTypeController(_context);
                
                pmttypelist = _controller.getPaymentTypes();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(pmttypelist.Count > 0)  
            {  
                return new OkObjectResult(pmttypelist);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Payment Types Not Found");  
            }  
        }

        
    }

     

}
