using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class UserController{

        private readonly DataContext _context;

        public UserController(DataContext _context)
        {
            this._context = _context;
        }

        public int AddUser(User user){
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;

        }

        public User UpdateUser(User user){
             _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }

        public List<User> GetUsers(){
           

            return _context.Users
                .ToList();
        }

        public User GetById(int userId){
            
            return _context.Users.Where(b => b.UserId == userId).FirstOrDefault();
            
        }

        
        public User GetByUserEmail(String userEmail){
            
            return _context.Users.Where(b => b.UserEmail == userEmail && b.IsActive).FirstOrDefault();
            
        }

        

        

    }

}