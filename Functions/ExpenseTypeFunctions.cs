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
    public  class ExpenseTypeFunctions
    {
        
        private readonly DataContext _context;
        public ExpenseTypeFunctions(DataContext context)
        {
            _context = context;
        }

        [FunctionName("GetExpenseTypes")]
        public  async Task<IActionResult> GetExpenseTypes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Expensetypes")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<ExpenseType> expensetypelist = new List<ExpenseType>();  
            try  
            {  
                ExpenseTypeController _controller = new ExpenseTypeController(_context);
                
                expensetypelist = _controller.GetExpenseTypes();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(expensetypelist.Count > 0)  
            {  
                return new OkObjectResult(expensetypelist);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Expense Types Not Found");  
            }  
        }

        
    }

     

}
