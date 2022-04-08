using System;
using System.Collections.Generic;
using Epam.Library.Entities;
using Epam.Library.Enums;
using Epam.Library.ErrorArchiver;

namespace Epam.Library.BLL.Interfaces
{
    public interface ILibraryLogic
    {
        IEnumerable<AbstractPaper> GetAll();

        IEnumerable<AbstractPaper> GetAllByType(PaperProducts products);

        AbstractPaper Add(AbstractPaper paperObject, out Response response);

        void Delete(PaperProducts products, int id, out Response response);

        AbstractPaper Update(AbstractPaper paperObject, out Response response);

        AbstractPaper GetById(PaperProducts products, int id);

        IEnumerable<AbstractPaper> GetByName(string name);

        IEnumerable<AbstractPaper> GroupByDate(int year);

        IEnumerable<AbstractPaper> GetOrderedByDesc(PaperProducts products);

        IEnumerable<AbstractPaper> GetOrderedBy(PaperProducts products);

        IEnumerable<AbstractPaper> GetAbstractPaperByAuthor(PaperProducts products, Person author);

        IEnumerable<AbstractPaper> GetAbstractPaperByNewspaperPubllishHouse(string publishingHouse);

        IEnumerable<AbstractPaper> GetByCharacterSet(PaperProducts products, string characterSet);

    }
}
