using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IBAM.API.Models{
     
    public class RegistrationType 
    {  
        
        public int RegistrationTypeId { get; set; }  
         
        public string Description { get; set; }
        public decimal Amount { get; set; }  
        public int EventId { get; set; }
        public Event Event {get;set;}
        
        public Boolean IsActive { get; set; }
        
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    } 
}