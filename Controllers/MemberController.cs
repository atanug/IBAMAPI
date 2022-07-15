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

        public int AddMember(Member member){
            _context.Members.Add(member);
            _context.SaveChanges();
            return member.MemberId;

        }

        public List<Member> GetMembers(string keyword){
           

            return _context.Members
                .Where(p => p.FirstName.ToLower().Contains(keyword) ||
                 p.LastName.ToLower().Contains(keyword)).ToList();
        }

        public Member GetMemberById(int memberId){
            return _context.Members.Where(b=>b.MemberId==memberId).FirstOrDefault();
        }

    }

}