using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class UserController{

        private readonly DataContext _context;

        public UserController(DataContext _context)
        {
            this._context = _context;
        }

        
        public User GetByUserEmail(String userEmail){
            
            return _context.Users.Where(b => b.UserEmail == userEmail && b.IsActive).FirstOrDefault();
            
        }

        

        

    }

}