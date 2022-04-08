using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System.Collections.Generic;

namespace Epam.Library.DAL.Interfaces
{
    public interface INewspaperDao
    {
        Newspaper AddNewspaper(Newspaper newspaper, ref Response response);

        Newspaper AddIssueToNewspaper(Issue newIssue, Newspaper newspaper, ref Response response);

        void DeleteNewspaper(int id, ref Response response);

        Newspaper UpdateNewspaper(Newspaper newspaper, ref Response response);

        Newspaper GetNewspaperById(int id);

        IEnumerable<Newspaper> GetAllNewspapers();

        IEnumerable<Newspaper> GetOrderedByDescNewspapers();

        IEnumerable<Newspaper> GetOrderedByNewspapers();

        IEnumerable<Newspaper> GetNewspapersByPublishingHouse(string publishingHouse);

        IEnumerable<Newspaper> GetNewspapersByCharacterSet(string characterSet);
    }
}
