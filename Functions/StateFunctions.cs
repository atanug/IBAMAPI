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
    public  class StateFunctions
    {
        
        private readonly DataContext _context;
        public StateFunctions(DataContext context)
        {
            _context = context;
        }

          

        [FunctionName("CreateState")]  
        public async Task<IActionResult> CreateState(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "states")] HttpRequest req, ILogger log)  
        {  
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<StateReq>(requestBody); 
            log.LogError(requestBody);

            try  
            {  

                 CountryController _countryController = new CountryController(_context);
                 Country country = _countryController.GetByCountryName(input.CountryName);

            State state = new State{
                StateName=input.StateName,
                
                CountryId=country.CountryId,
                IsDefault=true,
                IsActive=true,
                CreatedOn=System.DateTime.Now,
                UpdatedOn=System.DateTime.Now};

                StateController _controller = new StateController(_context);
                _controller.AddState(state);

            
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return new BadRequestResult();  
            }  
            return new BadRequestResult();  
        }  
        
    }

     class StateReq{
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }

}
