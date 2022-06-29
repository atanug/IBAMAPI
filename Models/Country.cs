using System;

namespace IBAM.API.Models{
    public class Country 
    {  
        public int CountryId { get; set; }  
         
        public string CountryName { get; set; }  
        public Boolean IsDefault { get; set; }
        public Boolean IsActive { get; set; }
        

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
          
    } 
}