using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;


namespace IBAM.API.Controllers{

    public class PaymentTypeController{

        private readonly DataContext _context;

        public PaymentTypeController(DataContext _context)
        {
            this._context = _context;
        }

        public List<PaymentType> getPaymentTypes(){
            return _context.PaymentTypes
                .Where(p => p.IsActive==true).ToList();
        }

        public PaymentType GetByDescription(String desc){
            
            return _context.PaymentTypes.Where(b => b.Description == desc).FirstOrDefault();
            
        }

        public PaymentType GetById(int paymentTypeId){
            
            return _context.PaymentTypes.Where(b => b.PaymentTypeId == paymentTypeId).FirstOrDefault();
            
        }

    }

}