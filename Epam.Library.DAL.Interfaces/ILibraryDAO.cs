using Epam.Library.Entities;
using System;
using System.Collections.Generic;

namespace Epam.Library.DAL.Interfaces
{
    public interface ILibraryDao
    {
        IEnumerable<AbstractPaper> GetAll();

        IEnumerable<AbstractPaper> GetByName(string name);

        IEnumerable<AbstractPaper> GroupByDate(int year);
    }
}
