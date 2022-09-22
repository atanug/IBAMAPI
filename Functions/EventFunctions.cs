using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    public  class EventFunctions
    {
        
        private readonly DataContext _context;
        public EventFunctions(DataContext context)
        {
            _context = context;
        }

        [FunctionName("GetEvents")]
        public  async Task<IActionResult> GetEvents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequest req,
            ILogger log)
        {
            
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            List<Event> events = new List<Event>();  
            try  
            {  
                EventController _controller = new EventController(_context);
                
                events = _controller.getEvents();
                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator"); 
            }  
            if(events.Count > 0)  
            {  
                var resp = JsonConvert.SerializeObject(events, Formatting.Indented,
                        new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                return new OkObjectResult(resp);  
            }  
            else  
            {  
                return ErrorResponse.NotFound(type: "/notfound",detail:"Events Not Found");  
            }  
        }

        [FunctionName("CreateEvent")]  
        public async Task<IActionResult> CreateEvent(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "events")] HttpRequest req, ILogger log)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Event>(requestBody); 
            log.LogError(requestBody);
            int EventId = 0;
            try  
            {  

                Event x = new Event{
                    EventName=input.EventName,
                    EventStartDate = Convert.ToDateTime(input.EventStartDate),
                    EventEndDate=Convert.ToDateTime(input.EventEndDate),
                    EventDescription = input.EventDescription,
                    IsActive = true,                
                    
                    CreatedOn=System.DateTime.Now,
                    UpdatedOn=System.DateTime.Now};

                EventController _controller = new EventController(_context);
                EventId = _controller.AddEvent(x);

                foreach (RegistrationType r in input.RegistrationTypes){
                    RegistrationTypeController _rcontroller = new RegistrationTypeController(_context);
                    r.EventId=EventId;
                    r.CreatedOn = System.DateTime.Now;
                    r.UpdatedOn=System.DateTime.Now;
                    _rcontroller.AddRegistrationType(r);
                    log.LogError(r.Description);
                }

            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"createEvent",detail:"Error Creating Event. Please contact system adminstrator."); 
            }  
            return new OkObjectResult(new { id=EventId}); ;  
        }

        [FunctionName("UpdateEvent")]  
        public async Task<IActionResult> UpdateEvent(  
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "events/{id}")] HttpRequest req, 
            ILogger log, int id)  
        {  

            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
            var input = JsonConvert.DeserializeObject<Event>(requestBody); 
            log.LogError(requestBody);

            EventController _eventcontroller = new EventController(_context);
            Event x = new Event();
            

            try{
                x = _eventcontroller.GetById(id);

                x.EventName=input.EventName;
                x.EventStartDate = Convert.ToDateTime(input.EventStartDate);
                x.EventEndDate=Convert.ToDateTime(input.EventEndDate);
                x.EventDescription = input.EventDescription;
                x.IsActive = input.IsActive;               
                
                x.UpdatedOn=System.DateTime.Now;
                

                _eventcontroller.UpdateEvent(x);
                _context.Entry(x).State = EntityState.Detached;

                foreach (RegistrationType r in input.RegistrationTypes){

                    RegistrationTypeController _rcontroller = new RegistrationTypeController(_context);
                    r.EventId=x.EventId;
                    r.UpdatedOn=System.DateTime.Now;
                    r.CreatedOn=System.DateTime.Now;
                    

                    if (r.RegistrationTypeId==0){
                        _rcontroller.AddRegistrationType(r);
                    }
                    else{
                        _rcontroller.UpdateRegistrationType(r);
                    }
                    _context.Entry(r).State = EntityState.Detached;
                    
                }

                
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.BadRequest(type:"updateEvent",detail:"Error Updating Event. Please contact system adminstrator."); 
            }  
            var resp = JsonConvert.SerializeObject(x, Formatting.Indented,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return new OkObjectResult(resp);
        }

        [FunctionName("GetEventById")]
        public  async Task<IActionResult> GetEventById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            // Check if we have authentication info.
            AuthenticationInfo auth = new AuthenticationInfo(req);
        
            if (!auth.IsValid)
            {
                return ErrorResponse.UnAuthorized(type:"authorization",detail:"Permission Denied"); 
            }
            
            Event x = new Event();
            
            try  
            {  
                EventController _controller = new EventController(_context);

                x = _controller.GetById(id);

                if (x==null){

                
                    return ErrorResponse.NotFound(type: "invalid_event_id",detail:"Event Not Found");
                      
                }
  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
                return ErrorResponse.InternalServerError(detail:"Internal Server Error. Please Contact System Adminstrator");
            }  
                return new OkObjectResult(x);  
              
        }
        
    }
}
