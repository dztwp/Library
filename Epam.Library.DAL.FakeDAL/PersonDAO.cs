using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System;
using System.Linq;

namespace Epam.Library.DAL.FakeDAL
{
    public class PersonDao : IPersonDao
    {
        public Person AddPerson(Person author, ref Response response)
        {
            author.Id = GetNewId();
            if (!PersonHelper.IsAuthorsListContainsAuthor(DataStorage.AuthorsStorage.Values, author))
            {
                DataStorage.AuthorsStorage.Add(author.Id, author);
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "This author already exist in data storage");
            }
            return author;
        }

        private int GetNewId()
        {
            return DataStorage.AuthorsStorage.Count + 1;
        }

        public void DeletePerson(int id, ref Response response)
        {
            if (DataStorage.AuthorsStorage.Remove(id))
            {
                DataStorage.Storage.Values.OfType<Book>()
                    .Select(x => x.Authors.ToList()
                        .RemoveAll(author => author.Id == id));
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
            }
        }

        public Person GetPersonById(int id)
        {
            return DataStorage.AuthorsStorage.Values
                    .FirstOrDefault(x => x.Id == id);
        }

        public Person UpdatePerson(Person author, ref Response response)
        {
            throw new NotImplementedException();
        }

        public int GetPersonIdByName(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
