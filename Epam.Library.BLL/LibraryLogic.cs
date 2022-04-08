using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.Enums;
using Epam.Library.ErrorArchiver;
using System;
using System.Collections.Generic;

namespace Epam.Library.BLL
{
    public class LibraryLogic : ILibraryLogic
    {
        private readonly IBookLogic _bookLogic;
        private readonly INewspaperLogic _newspaperLogic;
        private readonly IPatentLogic _patentLogic;
        private readonly ILibraryDao _libraryStorage;

        public LibraryLogic(IBookLogic bookLogic, INewspaperLogic newspaperLogic, IPatentLogic patentLogic, ILibraryDao libraryStorage)
        {
            _bookLogic = bookLogic;
            _newspaperLogic = newspaperLogic;
            _patentLogic = patentLogic;
            _libraryStorage = libraryStorage;
        }

        public AbstractPaper Add(AbstractPaper paperObject, out Response response)
        {
            response = new Response();

            if (paperObject is Book book)
            {
                return _bookLogic.AddBook(book, ref response);
            }
            else if (paperObject is Newspaper newspaper)
            {
                return _newspaperLogic.AddNewspaper(newspaper, ref response);
            }
            else if (paperObject is Patent patent)
            {
                return _patentLogic.AddPatent(patent, ref response);
            }
            return null;
        }

        public void Delete(PaperProducts products, int id, out Response response)
        {
            response = new Response();

            switch (products)
            {
                case PaperProducts.Book:
                    _bookLogic.DeleteBook(id, ref response);
                    break;
                case PaperProducts.Newspaper:
                    _newspaperLogic.DeleteNewspaper(id, ref response);
                    break;
                case PaperProducts.Patent:
                    _patentLogic.DeletePatent(id, ref response);
                    break;
            }
        }

        public IEnumerable<AbstractPaper> GetAbstractPaperByAuthor(PaperProducts products, Person author)
        {
            switch (products)
            {
                case PaperProducts.Book:
                    return _bookLogic.GetBooksByAuthor(author);
                case PaperProducts.Patent:
                    return _patentLogic.GetPatentsByAuthor(author);
                default:
                    return null;
            }
        }

        public IEnumerable<AbstractPaper> GetAbstractPaperByNewspaperPubllishHouse(string publishingHouse)
        {
            return _newspaperLogic.GetNewspapersByPublishingHouse(publishingHouse);
        }

        public IEnumerable<AbstractPaper> GetAll()
        {
            return _libraryStorage.GetAll();
        }

        public IEnumerable<AbstractPaper> GetAllByType(PaperProducts products)
        {
            switch (products)
            {
                case PaperProducts.Book:
                    return _bookLogic.GetAllBooks();
                case PaperProducts.Newspaper:
                    return _newspaperLogic.GetAllNewspapers();
                case PaperProducts.Patent:
                    return _patentLogic.GetAllPatents();
                default:
                    return null;
            }
        }

        public IEnumerable<AbstractPaper> GetByCharacterSet(PaperProducts products, string characterSet)
        {
            throw new NotImplementedException();
        }

        public AbstractPaper GetById(PaperProducts products, int id)
        {
            switch (products)
            {
                case PaperProducts.Book:
                    return _bookLogic.GetBookById(id);
                case PaperProducts.Newspaper:
                    return _newspaperLogic.GetNewspaperById(id);
                case PaperProducts.Patent:
                    return _patentLogic.GetPatentById(id);
                default:
                    return null;
            }
        }

        public IEnumerable<AbstractPaper> GetByName(string name)
        {
            return name != null ?
                _libraryStorage.GetByName(name):
                null;
        }

        public IEnumerable<AbstractPaper> GetOrderedBy(PaperProducts products)
        {
            switch (products)
            {
                case PaperProducts.Book:
                    return _bookLogic.GetOrderedByBooks();
                case PaperProducts.Newspaper:
                    return _newspaperLogic.GetOrderedByNewspapers();
                case PaperProducts.Patent:
                    return _patentLogic.GetOrderedByPatents();
                default:
                    return null;
            }
        }

        public IEnumerable<AbstractPaper> GetOrderedByDesc(PaperProducts products)
        {
            switch (products)
            {
                case PaperProducts.Book:
                    return _bookLogic.GetOrderedByDescBooks();
                case PaperProducts.Newspaper:
                    return _newspaperLogic.GetOrderedByDescNewspapers();
                case PaperProducts.Patent:
                    return _patentLogic.GetOrderedByDescPatents();
                default:
                    return null;
            }
        }

        public IEnumerable<AbstractPaper> GroupByDate(int year)
        {
            return _libraryStorage.GroupByDate(year);
        }

        public AbstractPaper Update(AbstractPaper paperObject, out Response response)
        {
            response = new Response();

            if (paperObject is Book)
            {
                return _bookLogic.UpdateBook(paperObject as Book, ref response);
            }
            else if (paperObject is Newspaper)
            {
                return _newspaperLogic.UpdateNewspaper(paperObject as Newspaper, ref response);
            }
            else
            {
                return _patentLogic.UpdatePatent(paperObject as Patent, ref response);
            }
        }
    }
}
