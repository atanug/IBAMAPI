using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IBAM.API.Models{
    public class User 
    {  
        
        public int UserId { get; set; }  
         
        public string UserEmail { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public Boolean IsActive { get; set; }
        

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
          
    } 
}