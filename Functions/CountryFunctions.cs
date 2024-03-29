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
    public  class CountryFunctions
    {
        
        private readonly DataContext _context;
        public CountryFunctions(DataContext context)
        {
            _context = context;
        }

         [FunctionName("CreateCountry")]  
        public async Task<IActionResult> CreateCountry(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "countries")] HttpRequest req, ILogger log)  
        {  
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Country>(requestBody);  

            

            try  
            {  
                
                
                Country country = new Country{CountryName=input.CountryName,IsDefault=true,IsActive=true,CreatedOn=System.DateTime.Now,UpdatedOn=System.DateTime.Now};
                CountryController _controller = new CountryController(_context);
                _controller.AddCountry(country);
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                //return new HttpResponseException(new HttpResponseMessage(e.ToString(), HttpStatusCode.BadRequest));
                return new BadRequestResult();  
            }  
            return new OkResult();  
        } 

        [FunctionName("GetCountries")]
        public  async Task<IActionResult> GetCountries(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Countries")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            // AuthenticationInfo auth = new AuthenticationInfo(req);
        
            // if (!auth.IsValid)
            // {
            //     return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            // }

            List<Country> Countries = new List<Country>();  
            try  
            {  
                CountryController _controller = new CountryController(_context);
                
                Countries = _controller.GetCountries();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(Countries.Count > 0)  
            {  
                return new OkObjectResult(Countries);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Countries Not Found");  
            }  
        }

        
    }

     

}
