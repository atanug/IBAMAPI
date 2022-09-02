using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class ExpenseController{

        private readonly DataContext _context;

        public ExpenseController(DataContext _context)
        {
            this._context = _context;
        }

        public int AddExpense(Expense expense){
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return expense.ExpenseId;

        }

        public Expense UpdateExpense(Expense expense){
             _context.Expenses.Attach(expense);
            _context.Entry(expense).State = EntityState.Modified;
            _context.SaveChanges();
            return expense;
        }

        public List<Expense> GetExpenses(int? eventId, int? memberId){
           

            var query= _context.Expenses
                .Where(p=> (eventId==null ||  p.EventId==eventId) && (memberId==null ||  p.MemberId==memberId) && p.IsActive==true)
                 .Include(i => i.Event)
                 .Include(c=>c.Member);

                 

                

                 return query.ToList();

                 
        }



    }

}