using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class MembershipTypeController{

        private readonly DataContext _context;

        public MembershipTypeController(DataContext _context)
        {
            this._context = _context;
        }

        public List<MembershipType> getMembershipTypes(){
            return _context.MembershipTypes
                .Where(p => p.IsActive==true).ToList();
        }

        public MembershipType GetByDesc(String desc){
            
            return _context.MembershipTypes.Where(b => b.Description == desc).FirstOrDefault();
            
        }



        public MembershipType GetById(int membershipTypeId){
            
            return _context.MembershipTypes.Where(b => b.MembershipTypeId == membershipTypeId).FirstOrDefault();
            
        }

    }

}