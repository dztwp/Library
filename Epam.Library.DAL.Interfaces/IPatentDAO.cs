using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System.Collections.Generic;

namespace Epam.Library.DAL.Interfaces
{
    public interface IPatentDao
    {
        Patent AddPatent(Patent patent, ref Response response);

        void DeletePatent(int Id, ref Response response);

        Patent UpdatePatent(Patent patent, ref Response response);

        Patent GetPatentById(int id);

        void AddPersonToPatent(int patentId, Person author);

        IEnumerable<Patent> GetAllPatents();

        IEnumerable<Patent> GetOrderedByDescPatents();

        IEnumerable<Patent> GetOrderedByPatents();

        IEnumerable<Patent> GetPatentsByAuthor(Person author);

        IEnumerable<Patent> GetPatentByCharacterSet(string characterSet);
    }
}
