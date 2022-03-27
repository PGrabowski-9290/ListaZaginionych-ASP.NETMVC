using ListaZaginionych.Data.Models;
using System.Collections.Generic;

namespace ListaZaginionych.Logic
{
    public interface ILostPeopleManager
    {
        ILostPeopleManager Add(LostPeopleModel model);
        ILostPeopleManager Remove(int id);
        ILostPeopleManager Update(LostPeopleModel model);
        LostPeopleModel Get(int id);
        List<LostPeopleModel> GetAll();
    }
}
