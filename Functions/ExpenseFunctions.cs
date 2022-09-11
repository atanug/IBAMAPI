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
            var input = JsonConvert.DeserializeObject<ExpenseReq>(requestBody); 
            log.LogError(requestBody);
            int expenseId = 0;
            try  
            {  
                UserController _usercontroller = new UserController(_context);
                User user = _usercontroller.GetByUserEmail(input.UserEmail);

                
                Expense expense = new Expense{
                ExpenseDescription=input.ExpenseDescription,    
                ExpenseTypeId=input.ExpenseTypeId,
                PaidBy=input.PaidBy,
                Amount = input.Amount,
                EventId=Convert.ToInt16(input.EventId),
                ExpenseDate=Convert.ToDateTime(input.ExpenseDate),
                Comments=input.Comments,
                Reimbursed=input.Reimbursed,
                IsActive = true,                
                CreatedOn=System.DateTime.Now,
                CreatedBy=user.UserId,
                UpdatedOn=System.DateTime.Now,
                UpdatedBy=user.UserId};

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

            
            try  
            {  
                ExpenseController _controller = new ExpenseController(_context);
                

                foreach (Expense expense in _controller.GetExpenses(eventId))
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

        [FunctionName("UpdateExpense")]  
        public async Task<IActionResult> UpdateExpense(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "expenses/{id}")] HttpRequest req, 
            ILogger log, int id)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<ExpenseReq>(requestBody); 
            log.LogError(requestBody);

            ExpenseController _expensecontroller = new ExpenseController(_context);
            Expense expense = new Expense();

            
            

            try{

                UserController _usercontroller = new UserController(_context);
                User user = _usercontroller.GetByUserEmail(input.UserEmail);


                expense = _expensecontroller.GetExpenseById(id);

                expense.ExpenseDescription=input.ExpenseDescription;   
                expense.ExpenseTypeId=input.ExpenseTypeId;
                expense.PaidBy=input.PaidBy;
                expense.Amount = input.Amount;
                expense.EventId=Convert.ToInt16(input.EventId);
                expense.ExpenseDate=Convert.ToDateTime(input.ExpenseDate);
                expense.Comments=input.Comments;
                expense.Reimbursed=input.Reimbursed;
                expense.IsActive = true ;              
                
                expense.UpdatedOn=System.DateTime.Now;
                expense.UpdatedBy=user.UserId;

                _expensecontroller.UpdateExpense(expense);

                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"updateexpense",detail:"Error Updating Expense Data. Please contact system adminstrator."); 
            } 

            
            
            return new OkObjectResult(_expensecontroller.GetExpenseById(id));
        }

        [FunctionName("GetExpenseById")]
        public  async Task<IActionResult> GetExpenseById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "expenses/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }
            
            Expense expense = new Expense();
            
            try  
            {  
                ExpenseController _controller = new ExpenseController(_context);

                expense = _controller.GetExpenseById(id);

                if (expense==null){

                
                    return ErrorResponse.NotFound(type: "invalid_expense_id",detail:"Expense Not Found");
                      
                }
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator");
            }  
                return new OkObjectResult(expense);  
              
        }


    }

public class ExpenseReq 
    {  
        
        public int ExpenseId { get; set; }  

        public string ExpenseDescription { get; set; }
        public int ExpenseTypeId { get; set; }
        public string PaidBy { get; set; }
        public decimal Amount { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Comments { get; set; }
        public Boolean Reimbursed { get; set; }

        
        public Boolean IsActive { get; set; }
        public DateTime CreatedOn { get; set; } 
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public string UserEmail { get; set; }

    }
    
}
