using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using FluentValidation;
using System.Collections.Generic;

namespace Epam.Library.BLL
{
    public class BookLogic : IBookLogic
    {
        private readonly IBookDao _bookStorage;
        private readonly IValidator<Book> _bookValidator;
        private readonly IPersonLogic _personLogic;

        public BookLogic(IBookDao bookStorage, IPersonLogic personLogic, IValidator<Book> bookValidator)
        {
            _bookStorage = bookStorage;
            _bookValidator = bookValidator;
            _personLogic = personLogic;
        }

        public Book AddBook(Book book, ref Response response)
        {
            if (IsNullBookCheck(book, ref response))
            {
                return book;
            }
            var validationResult = _bookValidator.Validate(book);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return book;
            }

            var returnedBook = _bookStorage.AddBook(book, ref response);
            response = AddingAuthorsToStorage(response, returnedBook);
            return returnedBook;

        }

        private Response AddingAuthorsToStorage(Response response, Book returnedBook)
        {
            if (response.IsSuccess)
            {
                foreach (var person in returnedBook.Authors)
                {
                    var retPerson = _personLogic.AddPerson(person, ref response);
                    if (retPerson.Id == 0)
                    {
                        retPerson.Id = _personLogic.GetPersonIdByName(retPerson.FirstName, retPerson.LastName);
                    }
                    AddPersonToBooks(returnedBook.Id, retPerson);
                }
            }
            return response;
        }

        private bool IsNullBookCheck(Book book, ref Response response)
        {
            if (book == null)
            {
                ErrorsManager.AddFalseResponse(ref response, "Book can not be null");
                return true;
            }
            return false;
        }

        public void DeleteBook(int id, ref Response response)
        {
            _bookStorage.DeleteBook(id, ref response);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookStorage.GetAllBooks();
        }

        public IEnumerable<Book> GetBooksByAuthor(Person author)
        {
            return author != null ?
                _bookStorage.GetBooksByAuthor(author) :
                null;
        }

        public Book GetBookById(int id)
        {
            return _bookStorage.GetBookById(id);
        }

        public IEnumerable<Book> GetBooksByCharacterSet(string characterSet)
        {
            return characterSet != null ?
                _bookStorage.GetBooksByCharacterSet(characterSet) :
                null;
        }

        public IEnumerable<Book> GetOrderedByBooks()
        {
            return _bookStorage.GetOrderedByBooks();
        }

        public IEnumerable<Book> GetOrderedByDescBooks()
        {
            return _bookStorage.GetOrderedByDescBooks();
        }

        public Book UpdateBook(Book book, ref Response response)
        {
            var validationResult = _bookValidator.Validate(book);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return book;
            }
            return _bookStorage.UpdateBook(book, ref response);

        }

        public void AddPersonToBooks(int booksId, Person author)
        {
            _bookStorage.AddPersonToBooks(booksId, author);
        }
    }
}
