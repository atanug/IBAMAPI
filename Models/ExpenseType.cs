using System;
using System.ComponentModel.DataAnnotations;


namespace IBAM.API.Models{

    public class ExpenseType{
        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }

       
        public Boolean IsActive { get; set; }
        

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
        
    }

    
}