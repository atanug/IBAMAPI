using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



using IBAM.API.Models;
using IBAM.API.Data;
using IBAM.API.Controllers;


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
                return new BadRequestResult();  
            }  
            return new OkResult();  
        } 

        
    }

     

}
