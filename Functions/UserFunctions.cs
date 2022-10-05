using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public  class UserFunctions
    {
        
        private readonly DataContext _context;
        public UserFunctions(DataContext context)
        {
            _context = context;
        }

          

        [FunctionName("Authenticate")]  
        public async Task<IActionResult> Authenticate(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "authenticate")] HttpRequest req, ILogger log)  
        {  
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<AuthenticateRequest>(requestBody); 
            log.LogError(requestBody);
            AuthenticateResponse _response ;
            try  
            {  

                UserController _controller = new UserController(_context);
                User user = _controller.GetByUserEmail(input.UserEmail);
                string token = JWTTokenGenerator.GenerateToken(user);
                _response = new AuthenticateResponse(user,token);

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.UnAuthorized(type:"authenticate",detail:"Invalid Admin Email"); 
            }  
            return new OkObjectResult(_response); 
        }  
        
        // [FunctionName("CreateUser")]  
        // public async Task<IActionResult> CreateUser(  
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req, ILogger log)  
        // {  

        //     // Check if we have authentication info.
        //     AuthenticationInfo auth = new AuthenticationInfo(req);
        
        //     if (!auth.IsValid)
        //     {
        //         return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
        //     }

        //     string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
        //     var input = JsonConvert.DeserializeObject<User>(requestBody); 
        //     //log.LogError(requestBody);
        //     int userId = 0;


        //     try  
        //     {  
        //         UserController _controller = new UserController(_context);

                

        //         User User = new User{
        //             UserEmail = input.UserEmail,
        //             FirstName = input.FirstName,
        //             LastName = input.LastName,                    
        //             CreatedOn=System.DateTime.Now,
        //             UpdatedOn=System.DateTime.Now,
        //             IsActive = true};

                
        //         userId = _controller.AddUser(User);

        //     }  
        //     catch (Exception e)  
        //     {  
        //         log.LogError(e.ToString());  
        //         return ErrorResponse.BadRequest(type:"createUser",detail:"Error Creating User. Please contact system adminstrator."); 
        //     }  
        //     return new OkObjectResult(new { id=userId}); ;  
        // } 
        
        [FunctionName("UpdateUsers")]  
        public async Task<IActionResult> UpdateUsers(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req, 
            ILogger log)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            JObject obj=JObject.Parse(requestBody);
            var token = (JArray)obj.SelectToken("users");


            List<User> userList = new List<User>();  
           

             try{

                var list=new List<User>();
                foreach (var item in token)
                {
                    string json = JsonConvert.SerializeObject(item);
                    list.Add(JsonConvert.DeserializeObject<User>(json));
                }

                UserController _controller = new UserController(_context);
                foreach (User u in list){
                    
                    u.UpdatedOn=System.DateTime.Now;
                    u.CreatedOn=System.DateTime.Now;

                    if (u.UserId==0){
                        _controller.AddUser(u);
                    }
                    else{
                        _controller.UpdateUser(u);
                    }
                    _context.Entry(u).State = EntityState.Detached;
                }

                userList = _controller.GetUsers();
             }
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"updateregistration",detail:"Error Updating User. Please contact system adminstrator."); 
            } 

            //return ErrorResponse.NotFound(type: "/notfound",detail:"Member Not Found");
            return new OkObjectResult(userList); 
        }

        [FunctionName("GetUsers")]
        public  async Task<IActionResult> GetUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<User> userList = new List<User>();  
            try  
            {  
                UserController _controller = new UserController(_context);
                
                userList = _controller.GetUsers();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(userList.Count > 0)  
            {  
                return new OkObjectResult(userList);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Users Not Found");  
            }  
        }

    }

      class AuthenticateRequest
    {
        
        public string UserEmail { get; set; }

        
    }

     class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserEmail = user.UserEmail;
            Token = token;
        }
    }

}
