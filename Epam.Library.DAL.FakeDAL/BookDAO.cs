using Epam.Library.DAL.FakeDAL;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.FakeDAL
{
    public class BookDao : IBookDao
    {
        public Book AddBook(Book book, ref Response response)
        {
            book.Id = GetNewId();
            if (IsISBNExistAndUnique(book) && IsBookUnique(book))
            {
                DataStorage.Storage.Add(book.Id, book);
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "Book with this ISBN or name, authors and year of publishing is already exist");
            }
            return book;
        }

        private int GetNewId()
        {
            return DataStorage.Storage.Count + 1;
        }

        private bool IsBookUnique(Book book)
        {
            return DataStorage.Storage.Values.OfType<Book>()
                .FirstOrDefault(x => x.Name == book.Name) == null &&
                DataStorage.Storage.Values.OfType<Book>()
                .FirstOrDefault(x => x.Authors == book.Authors) == null &&
                DataStorage.Storage.Values.OfType<Book>()
                .FirstOrDefault(x => x.YearOfPublishing == book.YearOfPublishing) == null;
        }

        private bool IsISBNExistAndUnique(Book book)
        {
            if (book.ISBN != null)
            {
                return DataStorage.Storage.OfType<Book>()
                    .FirstOrDefault(x => x.ISBN == book.ISBN) == null;
            }
            return true;
        }

        public void DeleteBook(int id, ref Response response)
        {
            if (!DataStorage.Storage.Remove(id))
            {
                ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
            }
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return DataStorage.Storage.Values.OfType<Book>().ToList();
        }

        public IEnumerable<Book> GetBooksByAuthor(Person author)
        {
            return DataStorage.Storage.Values.OfType<Book>()
                .Where(x => PersonHelper.IsAuthorsListContainsAuthor(x.Authors, author));
        }


        public Book GetBookById(int id)
        {
            return DataStorage.Storage.Values.OfType<Book>()
                    .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Book> GetBooksByCharacterSet(string characterSet)
        {
            return DataStorage.Storage.Values.OfType<Book>()
                .Where(x => x.Name.StartsWith(characterSet)).ToList();
        }

        public IEnumerable<Book> GetOrderedByBooks()
        {
            return DataStorage.Storage.Values.OfType<Book>()
                .OrderBy(x => x.YearOfPublishing).ToList();
        }

        public IEnumerable<Book> GetOrderedByDescBooks()
        {
            return DataStorage.Storage.Values.OfType<Book>()
                .OrderByDescending(x => x.YearOfPublishing).ToList();
        }

        public Book UpdateBook(Book book, ref Response response)
        {
            throw new System.NotImplementedException();
        }

        public void AddPersonToBooks(int bookId, Person author)
        {
            throw new System.NotImplementedException();
        }
    }
}
