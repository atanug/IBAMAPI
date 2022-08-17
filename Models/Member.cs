using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IBAM.API.Models{
    public class Member 
    {  
        public int MemberId { get; set; }  
         
        public string FirstName { get; set; }  
        public string LastName{get;set;}
        public string StreetAddress1{get;set;}
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        
        
        public int StateId { get; set; }
        public State State { get; set; }

       
        public int CountryId { get; set; }
        public Country Country { get; set; }    

        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        public Boolean IsActiveMember { get; set; }
          
    } 

    



}