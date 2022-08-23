using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class RegistrationTypeController{

        private readonly DataContext _context;

        public RegistrationTypeController(DataContext _context)
        {
            this._context = _context;
        }

        public List<RegistrationType> getRegistrationTypes(){
            return _context.RegistrationTypes
                .Where(p => p.IsActive==true).ToList();
        }

        public RegistrationType GetByDesc(String desc){
            
            return _context.RegistrationTypes.Where(b => b.Description == desc && b.IsActive==true).FirstOrDefault();
            
        }

        public List<RegistrationType> getByEventId(int eventId){
            return _context.RegistrationTypes
                .Where(p => p.IsActive==true && p.EventId==eventId).ToList();
        }

        public RegistrationType GetById(int registrationTypeId){
            
            return _context.RegistrationTypes.Where(b => b.RegistrationTypeId == registrationTypeId && b.IsActive==true).FirstOrDefault();
            
        }

    }

}