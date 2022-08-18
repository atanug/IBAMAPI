using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class MembershipController{

        private readonly DataContext _context;

        public MembershipController(DataContext _context)
        {
            this._context = _context;
        }

        public int AddMembership(Membership membership){
            _context.Memberships.Add(membership);
            _context.SaveChanges();
            return membership.MembershipId;

        }

        public List<Membership> GetMembershipsbyMember(int memberId){
           

            return _context.Memberships
                .Where(p => p.MemberId==memberId && p.IsActive==true)
                 .Include(i => i.PaymentType)
                 .Include(c=>c.MembershipType)
                 .OrderByDescending(d=>d.TransactionDate)
                 .ToList();
        }

    }

}