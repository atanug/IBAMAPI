using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class EventController{

        private readonly DataContext _context;

        public EventController(DataContext _context)
        {
            this._context = _context;
        }

        

        public List<Event> getEvents(){
            return _context.Events
                .Where(p => p.IsActive==true).
                Include(r=>r.RegistrationTypes).
                ToList();
        }

        public Event GetByDescription(String desc){
            
            return _context.Events.Where(b => b.EventDescription == desc).FirstOrDefault();
            
        }

        public Event GetById(int eventId){
            
            return _context.Events.Where(b => b.EventId == eventId).
            Include(r=>r.RegistrationTypes).
            FirstOrDefault();
            
        }

        public int AddEvent(Event x) {

            _context.Events.Add(x);
            _context.SaveChanges();
            return x.EventId;

        }

        public Event UpdateEvent(Event x){
             _context.Events.Attach(x);
            _context.Entry(x).State = EntityState.Modified;
            _context.SaveChanges();
            return x;
        }

    }

}