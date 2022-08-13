using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IBAM.API.Models{
    public class Membership 
    {  
        
        public int MembershipId { get; set; }  

        public int MemberId { get; set; }
        public decimal Amount { get; set; }
        public int PmtTypeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comments { get; set; }
        public int MembershipTypeId { get; set; }
        public Boolean IsActive { get; set; }
        
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    } 
}