using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;

namespace Epam.Library.BLL.Interfaces
{
    public interface IPersonLogic
    {
        Person AddPerson(Person author, ref Response response);

        void DeletePerson(int id, ref Response response);

        Person UpdatePerson(Person author, ref Response response);

        Person GetPersonById(int id);

        int GetPersonIdByName(string firstName, string lastName);
    }
}
