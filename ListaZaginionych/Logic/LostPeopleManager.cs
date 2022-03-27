using System;
using System.Linq;
using System.Collections.Generic;
using ListaZaginionych.Data.Models;
using ListaZaginionych.Data;

namespace ListaZaginionych.Logic
{
    public class LostPeopleManager : ILostPeopleManager
    {
        private readonly ApplicationDbContext _context;

        public LostPeopleManager(ApplicationDbContext context)
        {
            _context = context;
        }
        public ILostPeopleManager Add(LostPeopleModel model)
        {
            _context.LostPeople.Add(model);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                model.Id = 0;
                _context.LostPeople.Add(model);
                _context.SaveChanges();
            }

            return this;
        }

        public LostPeopleModel Get(int id)
        {
            return _context.LostPeople.FirstOrDefault(x => x.Id == id);
        }

        public List<LostPeopleModel> GetAll()
        {
            return _context.LostPeople.ToList();
        }

        public ILostPeopleManager Remove(int id)
        {
            var delObj = _context.LostPeople.SingleOrDefault(x => x.Id == id);
            if (delObj != null)
            {
                _context.LostPeople.Remove(delObj);
                _context.SaveChanges();
            }

            return this;
        }

        public ILostPeopleManager Update(LostPeopleModel model)
        {
            _context.LostPeople.Update(model);
            _context.SaveChanges();

            return this;
        }
    }
}
