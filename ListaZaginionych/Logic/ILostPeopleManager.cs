using ListaZaginionych.Data.Models;
using System.Linq;

namespace ListaZaginionych.Logic
{
    public interface ILostPeopleManager
    {
        ILostPeopleManager Add(LostPeopleModel model);
        ILostPeopleManager Remove(int id);
        ILostPeopleManager Update(LostPeopleModel model);
        LostPeopleModel Get(int id);
        IQueryable<LostPeopleModel> GetAll();
    }
}
