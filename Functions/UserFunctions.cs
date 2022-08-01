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
                _response = new AuthenticateResponse(user,"12345");

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.UnAuthorized(type:"authenticate",detail:"Invalid Admin Email"); 
            }  
            return new OkObjectResult(_response); 
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
