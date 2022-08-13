using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IBAM.API.Models{
    public class PaymentType 
    {  
        
        public int PaymentTypeId { get; set; }  
         
        public string Description { get; set; }
        public Boolean IsActive { get; set; }
        
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    } 
}