using System;
using System.Collections.Generic;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;

namespace Epam.Library.BLL.Interfaces
{
    public interface IPatentLogic
    {
        Patent AddPatent(Patent patent,ref Response response);

        void DeletePatent(int Id, ref Response response);

        Patent UpdatePatent(Patent patent, ref Response response);

        Patent GetPatentById(int id);

        void AddPersonToPatent(int patentId, Person author);

        IEnumerable<Patent> GetAllPatents();

        IEnumerable<Patent> GetOrderedByDescPatents();

        IEnumerable<Patent> GetOrderedByPatents();

        IEnumerable<Patent> GetPatentsByAuthor(Person author);

        IEnumerable<Patent> GetPatentsByCharacterSet(string characterSet);
    }
}
