using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

using IBAM.API.Data;
using IBAM.API.Models;

namespace IBAM.API.Controllers{

    public class CountryController{

        private readonly DataContext _context;

        public CountryController(DataContext _context)
        {
            this._context = _context;
        }

        public void AddCountry(Country country){
            _context.Countries.Add(country);
            _context.SaveChanges();

        }

        public List<Country> GetCountries(){
            return _context.Countries
                .Where(p => p.IsActive==true).ToList();
        }

        public Country GetByCountryName(String countryName){
            
            return _context.Countries.Where(b => b.CountryName == countryName).FirstOrDefault();
            
        }

        public Country GetById(int countryId){
            
            return _context.Countries.Where(b => b.CountryId == countryId).FirstOrDefault();
            
        }

    }

}