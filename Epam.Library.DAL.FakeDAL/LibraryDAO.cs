using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.DAL.FakeDAL
{
    public class LibraryDao : ILibraryDao
    {
        public IEnumerable<AbstractPaper> GetAll()
        {
            return DataStorage.Storage.Values.ToList();
        }

        public IEnumerable<AbstractPaper> GetByName(string name)
        {
            return DataStorage.Storage.Values
                    .Where(x => x.Name == name).ToList();
        }

        public IEnumerable<AbstractPaper> GroupByDate(int year)
        {
            return DataStorage.Storage.Values
                .Where(x => x.YearOfPublishing
                == year).ToList();
        }
    }
}
