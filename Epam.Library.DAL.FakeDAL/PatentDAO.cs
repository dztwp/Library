using Epam.Library.DAL.FakeDAL;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.FakeDAL
{
    public class PatentDao : IPatentDao
    {
        public Patent AddPatent(Patent patent, ref Response response)
        {
            patent.Id = GetNewId();

            if (IsPatentUnique(patent))
            {
                DataStorage.Storage.Add(patent.Id,patent);
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "Patent with this registration number and country is already exist");
            }
            return patent;
        }

        private int GetNewId()
        {
            return DataStorage.Storage.Count + 1;
        }

        private bool IsPatentUnique(Patent patent)
        {
            return DataStorage.Storage.Values.OfType<Patent>()
                .FirstOrDefault(x => x.RegistrationNumber == patent.RegistrationNumber)==null &&
                DataStorage.Storage.Values.OfType<Patent>()
                .FirstOrDefault(x => x.Country == patent.Country)==null;
        }

        public void DeletePatent(int Id, ref Response response)
        {
            if (!DataStorage.Storage.Remove(Id))
            {
                ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
            }
        }

        public IEnumerable<Patent> GetAllPatents()
        {
            return DataStorage.Storage.Values.OfType<Patent>().ToList();
        }

        public IEnumerable<Patent> GetPatentsByAuthor(Person author)
        {
            return DataStorage.Storage.Values.OfType<Patent>()
                .Where(x => PersonHelper.IsAuthorsListContainsAuthor(x.Authors, author));
        }

        public IEnumerable<Patent> GetPatentByCharacterSet(string characterSet)
        {
            return DataStorage.Storage.Values.OfType<Patent>()
                .Where(x => x.Name.StartsWith(characterSet));
        }

        public IEnumerable<Patent> GetOrderedByPatents()
        {
            return DataStorage.Storage.Values.OfType<Patent>()
                .OrderBy(x => x.YearOfPublishing)
                .ToList();
        }

        public IEnumerable<Patent> GetOrderedByDescPatents()
        {
            return DataStorage.Storage.Values.OfType<Patent>()
                .OrderBy(x => x.YearOfPublishing)
                .ToList();
        }

        public Patent GetPatentById(int id)
        {
            return DataStorage.Storage.Values.OfType<Patent>()
                    .FirstOrDefault(x => x.Id == id);
        }

        public Patent UpdatePatent(Patent patent, ref Response response)
        {
            throw new NotImplementedException();
        }

        public void AddPersonToPatent(int patentId, Person author)
        {
            throw new NotImplementedException();
        }
    }
}
