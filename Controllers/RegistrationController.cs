using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;


using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class RegistrationController{

        private readonly DataContext _context;

        public RegistrationController(DataContext _context)
        {
            this._context = _context;
        }

        public int AddRegistration(Registration registration){
            _context.Registrations.Add(registration);
            _context.SaveChanges();
            return registration.RegistrationId;

        }

        public Registration UpdateRegistration(Registration registration){
            _context.Registrations.Attach(registration);
            _context.Entry(registration).State = EntityState.Modified;
            _context.SaveChanges();
            return registration;
        }

        public List<Registration> GetRegistrationsbyMember(int memberId){
           
            

            return _context.Registrations
                .Where(p => p.MemberId==memberId && p.IsActive==true)
                 .Include(i => i.PaymentType)
                 .Include(c=>c.RegistrationType)
                 .ThenInclude(x => x.Event)
                 .OrderByDescending(d=>d.TransactionDate)
                 .ToList();
        }

        public Registration GetRegistrationbyId(int registrationId){
           
            

            return _context.Registrations
                .Where(p => p.RegistrationId==registrationId && p.IsActive==true)
                 .Include(m=>m.Member)
                 .Include(i => i.PaymentType)
                 .Include(c=>c.RegistrationType)
                 .ThenInclude(x => x.Event)
                 .FirstOrDefault();
        }
       

    }

}