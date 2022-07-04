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
    public  class MemberFunctions
    {
        
        private readonly DataContext _context;
        public MemberFunctions(DataContext context)
        {
            _context = context;
        }

          

        [FunctionName("CreateMember")]  
        public async Task<IActionResult> CreateMember(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "members")] HttpRequest req, ILogger log)  
        {  
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<MemberReq>(requestBody); 
            log.LogError(requestBody);

            try  
            {  

                CountryController _countryController = new CountryController(_context);
                Country country = _countryController.GetByCountryName(input.CountryName);

                StateController _stateController = new StateController(_context);
                State state = _stateController.getByStateName(input.StateName);

                Member member = new Member{
                    FirstName=input.FirstName,
                    LastName = input.LastName,
                    StreetAddress1=input.StreetAddress1,
                    StreetAddress2=input.StreetAddress2,
                    City=input.City,
                    PostalCode=input.PostalCode,
                    StateId=state.StateId,
                    CountryId=country.CountryId,
                    PhoneNumber=input.PhoneNumber,
                    EmailAddress=input.EmailAddress,
                
                    
                    CreatedOn=System.DateTime.Now,
                    UpdatedOn=System.DateTime.Now};

                MemberController _controller = new MemberController(_context);
                _controller.AddMember(member);

            
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return new BadRequestResult();  
            }  
            return new OkResult();  
        }  
        
    }

     class MemberReq{
        public string FirstName { get; set; }  
        public string LastName{get;set;}
        public string StreetAddress1{get;set;}
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        
        public string StateName { get; set; }
        public string CountryName { get; set; }

        
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        
    }

}
