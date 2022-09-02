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
    public  class ExpenseFunctions
    {
        
        private readonly DataContext _context;
        public ExpenseFunctions(DataContext context)
        {
            _context = context;
        }

          

        [FunctionName("CreateExpense")]  
        public async Task<IActionResult> CreateExpense(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "expenses")] HttpRequest req, ILogger log)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Expense>(requestBody); 
            log.LogError(requestBody);
            int expenseId = 0;
            try  
            {  


                Expense expense = new Expense{
                ExpenseDescription=input.ExpenseDescription,    
                MemberId=input.MemberId,
                Amount = input.Amount,
                EventId=Convert.ToInt16(input.EventId),
                ExpenseDate=Convert.ToDateTime(input.ExpenseDate),
                Comments=input.Comments,
                Reimbursed=input.Reimbursed,
                IsActive = true,                
                CreatedOn=System.DateTime.Now,
                CreatedBy=input.CreatedBy,
                UpdatedOn=System.DateTime.Now,
                UpdatedBy=input.UpdatedBy};

                ExpenseController _controller = new ExpenseController(_context);
                expenseId = _controller.AddExpense(expense);

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"createExpense",detail:"Error Creating Expense record. Please contact system adminstrator."); 
            }  
            return new OkObjectResult(new { id=expenseId}); ;  
        }  
        
        [FunctionName("GetExpenses")]
        public  async Task<IActionResult> GetExpenses(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "expenses")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<Expense> ExpenseList = new List<Expense>();  

            
            int? eventId =null;
            int? memberId=null;

            if (req.Query["eventid"].ToString().Trim()!="") {

             eventId = Convert.ToInt16(req.Query["eventid"]);
            }

            if (req.Query["memberId"].ToString().Trim()!="") {
             memberId = Convert.ToInt16(req.Query["memberId"]);
            }
            try  
            {  
                ExpenseController _controller = new ExpenseController(_context);
                

                foreach (Expense expense in _controller.GetExpenses(eventId, memberId))
                {
                    ExpenseList.Add(expense);
                }
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(ExpenseList.Count > 0)  
            {  
                return new OkObjectResult(ExpenseList);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Expense information Not Found");  
            }  
        }

        

    }

    
}
