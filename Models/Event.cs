using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IBAM.API.Models{
    //[JsonObject(IsReference = true)] 
    public class Event 
    {  
        
        public int EventId { get; set; }  
         
        public string EventName { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string EventDescription { get; set; }

        
        public ICollection<RegistrationType> RegistrationTypes { get; set; }

      
        public Boolean IsActive { get; set; }
        
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    } 
}