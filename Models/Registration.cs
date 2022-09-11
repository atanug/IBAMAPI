using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IBAM.API.Models{
    public class Registration 
    {  
        
        public int RegistrationId { get; set; }  

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int RegistrationTypeId { get; set; }
        public RegistrationType RegistrationType { get; set; }
        public decimal Amount { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType{get;set;}
        public int NoOfPeople { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comments { get; set; }
        
        public Boolean IsActive { get; set; }
        
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    } 
}