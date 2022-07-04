using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class MemberController{

        private readonly DataContext _context;

        public MemberController(DataContext _context)
        {
            this._context = _context;
        }

        public void AddMember(Member member){
            _context.Members.Add(member);
            _context.SaveChanges();

        }

        public Country GetByCountryName(String countryName){
            
            return _context.Countries.Where(b => b.CountryName == countryName).FirstOrDefault();
            
        }

    }

}