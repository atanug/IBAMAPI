using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IBAM.API.Models{
    public class Expense 
    {  
        
        public int ExpenseId { get; set; }  

        public string ExpenseDescription { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public decimal Amount { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Comments { get; set; }
        public Boolean Reimbursed { get; set; }

        
        public Boolean IsActive { get; set; }
        public DateTime CreatedOn { get; set; } 
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    } 
}