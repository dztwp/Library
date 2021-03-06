using System;
using System.Collections.Generic;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;

namespace Epam.Library.BLL.Interfaces
{
    public interface IBookLogic
    {
        Book AddBook(Book book, ref Response response);

        void DeleteBook(int id, ref Response response);

        Book UpdateBook(Book book, ref Response response);

        Book GetBookById(int id);

        void AddPersonToBooks(int booksId, Person author);

        IEnumerable<Book> GetAllBooks();

        IEnumerable<Book> GetOrderedByDescBooks();

        IEnumerable<Book> GetOrderedByBooks();

        IEnumerable<Book> GetBooksByAuthor(Person author);

        IEnumerable<Book> GetBooksByCharacterSet(string characterSet);

    }
}
