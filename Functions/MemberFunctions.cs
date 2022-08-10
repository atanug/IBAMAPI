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

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<MemberReq>(requestBody); 
            log.LogError(requestBody);
            int memberId = 0;
            try  
            {  

                CountryController _countryController = new CountryController(_context);
                Country country = _countryController.GetByCountryName(input.CountryName);

                StateController _stateController = new StateController(_context);
                State state = _stateController.GetByStateName(input.StateName);

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
                 memberId = _controller.AddMember(member);

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"createmember",detail:"Error Creating Member. Please contact system adminstrator."); 
            }  
            return new OkObjectResult(new { id=memberId}); ;  
        }  
        
        
        [FunctionName("GetMembers")]
        public  async Task<IActionResult> GetMembers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "members/KEY/{keyword}")] HttpRequest req,
            ILogger log, string keyword)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<MemberReq> MemberList = new List<MemberReq>();  
            try  
            {  
                MemberController _controller = new MemberController(_context);
                

                foreach (Member member in _controller.GetMembers(keyword))
                {

                     CountryController _countryController = new CountryController(_context);
                     Country country = _countryController.GetById(member.CountryId);

                    StateController _stateController = new StateController(_context);
                    State state = _stateController.getById(member.StateId);

                    MemberReq memberReq = new MemberReq{
                        MemberId = member.MemberId,
                        FirstName=member.FirstName,
                        LastName=member.LastName,
                        StreetAddress1 = member.StreetAddress1,
                        StreetAddress2 = member.StreetAddress2,
                        City = member.City,
                        PostalCode = member.PostalCode,
                        EmailAddress = member.EmailAddress,
                        PhoneNumber = member.PhoneNumber,
                        StateName = state.StateName,
                        CountryName=country.CountryName     
                    };

                    MemberList.Add(memberReq);
                }
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(MemberList.Count > 0)  
            {  
                return new OkObjectResult(MemberList);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Member Not Found");  
            }  
        }

        [FunctionName("UpdateMember")]  
        public async Task<IActionResult> UpdateMember(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "members/{id}")] HttpRequest req, 
            ILogger log, int id)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<MemberReq>(requestBody); 

            MemberController _membercontroller = new MemberController(_context);
            CountryController _countryController = new CountryController(_context);
            StateController _stateController = new StateController(_context);
            Member member = new Member();
            MemberReq resp = new MemberReq();

            try{
                 member = _membercontroller.GetMemberById(id);

                Country country = _countryController.GetByCountryName(input.CountryName);
                State state = _stateController.GetByStateName(input.StateName);

                member.FirstName=input.FirstName;
                    member.LastName = input.LastName;
                    member.StreetAddress1=input.StreetAddress1;
                    member.StreetAddress2=input.StreetAddress2;
                    member.City=input.City;
                    member.PostalCode=input.PostalCode;
                    member.StateId=state.StateId;
                    member.CountryId=country.CountryId;
                    member.PhoneNumber=input.PhoneNumber;
                    member.EmailAddress=input.EmailAddress;                
                    member.UpdatedOn=System.DateTime.Now;

                    _membercontroller.UpdateMember(member);

                    resp = ConvertFromMember(member,country,state);
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"updatemember",detail:"Error Updating Member. Please contact system adminstrator."); 
            }  
            return new OkObjectResult(resp);
        }



        [FunctionName("GetMemberById")]
        public  async Task<IActionResult> GetMemberById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "members/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }
            
            MemberReq memberReq = new MemberReq();
            
            try  
            {  
                MemberController _controller = new MemberController(_context);

                Member member = _controller.GetMemberById(id);

                if (member!=null){

                CountryController _countryController = new CountryController(_context);
                Country country = _countryController.GetById(member.CountryId);

                StateController _stateController = new StateController(_context);
                State state = _stateController.getById(member.StateId);

                
                    memberReq = ConvertFromMember(member, country,state);        
                }
                else{
                    return ErrorResponse.NotFound(type: "invalid_member_id",detail:"Member Not Found");
                      
                }
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator");
            }  
            if(memberReq!=null)  
            {  
                return new OkObjectResult(memberReq);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "invalid_member_id",detail:"Member Not Found"); 
            }  
        }

        private MemberReq ConvertFromMember(Member member, Country country, State state)
        {
            MemberReq req = new MemberReq();

            
                req.MemberId = member.MemberId;
                req.FirstName=member.FirstName;
                req.LastName=member.LastName;
                req.StreetAddress1 = member.StreetAddress1;
                req.StreetAddress2 = member.StreetAddress2;
                req.City = member.City;
                req.PostalCode = member.PostalCode;
                req.EmailAddress = member.EmailAddress;
                req.PhoneNumber = member.PhoneNumber;
                req.StateName = state.StateName;
                req.CountryName=country.CountryName; 

                return req;
        } 

    }

     public class MemberReq{

        public MemberReq(){

        }
        
        
        public int MemberId {get;set;}
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
