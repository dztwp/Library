using Epam.Library.DAL.FakeDAL;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.FakeDAL
{
    public class NewspaperDao : INewspaperDao
    {
        public Newspaper AddNewspaper(Newspaper newspaper, ref Response response)
        {
            if (IsISSNCorrect(newspaper))
            {
                return NewspaperAddingToCollection(newspaper, ref response);
            }
            ErrorsManager.AddFalseResponse(ref response, "Newspaper with this ISSN already exist, but the names don't match");
            return newspaper;
        }

        public Newspaper AddIssueToNewspaper(Issue newIssue, Newspaper newspaper, ref Response response)
        {
            if (IsStorageContainsNewspaper(newspaper) && IsNewspaperDoNotContainsIssue(newIssue, ref response))
            {
                DataStorage.Storage.Values.OfType<Newspaper>()
                    .FirstOrDefault(x => x.Id == newspaper.Id)
                    .CollectionOfIssues.ToList().Add(newIssue);
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "The specified newspaper does not exist in the data storage");
            }
            return newspaper;
        }

        private bool IsNewspaperDoNotContainsIssue(Issue newIssue, ref Response response)
        {
            var IsNewspaperContainsIssue = DataStorage.Storage.Values.OfType<Newspaper>().
                Any(x => x.CollectionOfIssues.Any(y => y.Id == newIssue.Id));
                
            if (IsNewspaperContainsIssue)
            {
                ErrorsManager.AddFalseResponse(ref response, "That issue already exist in choosed newspaper");
                return false;
            }
            return true;
        }

        private static bool IsStorageContainsNewspaper(Newspaper newspaper)
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                            .FirstOrDefault(x => x.Id == newspaper.Id) != null;
        }

        private bool IsISSNCorrect(Newspaper newspaper)
        {
            if (IsEnteredISSNEqualsISSNinStorage(newspaper))
            {
                return DataStorage.Storage.Values.OfType<Newspaper>().FirstOrDefault(x => x.ISSN == newspaper.ISSN && x.Name == newspaper.Name) != null;
            }
            return true;
        }

        private static bool IsEnteredISSNEqualsISSNinStorage(Newspaper newspaper)
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                            .FirstOrDefault(x => x.ISSN == newspaper.ISSN) != null;
        }

        private Newspaper NewspaperAddingToCollection(Newspaper newspaper, ref Response response)
        {
            newspaper.Id = GetNewId();

            if (IsNewspaperUnique(newspaper))
            {
                DataStorage.Storage.Add(newspaper.Id, newspaper);
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "Newspaper with this name, publishing house and year of publishing is already exist");
            }
            return newspaper;
        }

        private int GetNewId()
        {
            return DataStorage.Storage.Count + 1;
        }

        private bool IsNewspaperUnique(Newspaper newspaper)
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                .FirstOrDefault(x => x.Name == newspaper.Name) == null &&
                DataStorage.Storage.Values.OfType<Newspaper>()
                .FirstOrDefault(x => x.PublishingHouse == newspaper.PublishingHouse) == null &&
                DataStorage.Storage.Values.OfType<Newspaper>()
                .FirstOrDefault(x => x.YearOfPublishing == newspaper.YearOfPublishing) == null;
        }

        public void DeleteNewspaper(int id, ref Response response)
        {
            if (!DataStorage.Storage.Remove(id))
            {
                ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
            }
        }

        public IEnumerable<Newspaper> GetAllNewspapers()
        {
            return DataStorage.Storage.Values.OfType<Newspaper>().ToList();
        }

        public IEnumerable<Newspaper> GetNewspapersByPublishingHouse(string publishingHouse)
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                .Where(x => x.PublishingHouse == publishingHouse)
                .ToList();
        }

        public Newspaper GetNewspaperById(int id)
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                    .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Newspaper> GetNewspapersByCharacterSet(string characterSet)
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                .Where(x => x.Name.StartsWith(characterSet)).ToList();
        }

        public IEnumerable<Newspaper> GetOrderedByNewspapers()
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                .OrderBy(x => x.YearOfPublishing)
                .ToList();
        }

        public IEnumerable<Newspaper> GetOrderedByDescNewspapers()
        {
            return DataStorage.Storage.Values.OfType<Newspaper>()
                .OrderByDescending(x => x.YearOfPublishing)
                .ToList();
        }

        public Newspaper UpdateNewspaper(Newspaper newspaper, ref Response response)
        {
            throw new NotImplementedException();
        }
    }
}
