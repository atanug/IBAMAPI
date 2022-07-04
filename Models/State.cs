using System;
using System.ComponentModel.DataAnnotations;


namespace IBAM.API.Models{

    public class State{
        public int StateId { get; set; }
        public String StateName { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
        
        public Boolean IsDefault { get; set; }
        public Boolean IsActive { get; set; }
        

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    }

    
}