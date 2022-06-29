using System;

namespace IBAM.API.Models{
    public class Member 
    {  
        public int MemberId { get; set; }  
         
        public string FirstName { get; set; }  
        public string LastName{get;set;}
        public string StreetAddress1{get;set;}
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        public Boolean IsActivemember { get; set; }
          
    } 

    public class MemberDisplay 
    {  
        public int MemberId { get; set; }  
         
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

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        public Boolean IsActivemember { get; set; }
          
    } 

}