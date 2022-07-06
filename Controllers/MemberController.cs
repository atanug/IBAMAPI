using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

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

        public List<Member> GetMembers(string keyword){

            return _context.Members
                .Where(p => p.FirstName.ToLower().Contains(keyword) ||
                 p.LastName.ToLower().Contains(keyword)).ToList();
        }

    }

}