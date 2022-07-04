using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class StateController{

        private readonly DataContext _context;

        public StateController(DataContext _context)
        {
            this._context = _context;
        }

        public void AddState(State state){
            _context.States.Add(state);
            _context.SaveChanges();

        }

        public State getByStateName(String stateName){
            return _context.States.Where(b=>b.StateName==stateName).FirstOrDefault();
        }

    }

}