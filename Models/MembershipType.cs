using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IBAM.API.Models{
    public class MembershipType 
    {  
        
        public int MembershipTypeId { get; set; }  
         
        public string Description { get; set; }
        public decimal Amount { get; set; }  
        public Boolean IsActive { get; set; }
        
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    } 
}