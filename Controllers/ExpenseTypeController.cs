using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class ExpenseTypeController{

        private readonly DataContext _context;

        public ExpenseTypeController(DataContext _context)
        {
            this._context = _context;
        }

        public void AddExpenseType(ExpenseType expenseType){
            _context.ExpenseTypes.Add(expenseType);
            _context.SaveChanges();

        }

        public List<ExpenseType> GetExpenseTypes(){
            return _context.ExpenseTypes
                .Where(p => p.IsActive==true).ToList();
        }

       

    }

}