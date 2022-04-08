using NUnit.Framework;
using Epam.Library.Entities;
using System;
using System.Collections.Generic;
using Epam.Library.BLL.Interfaces;
using Epam.Library.Dependencies;
using Ninject;
using Epam.Library.ErrorArchiver;
using System.Linq;
using Epam.Library.DAL.FakeDAL;

namespace Epam.Library.BLL.IntegrationTests
{
    public class BookIntegrationTests
    {
        private Book _correctBook;
        private IBookLogic _bookLogic;
        private Response _response;

        [SetUp]
        public void Setup()
        {
            DataStorage.Storage.Clear();
            _correctBook = new Book
            {
                Name = "Jija",
                Note = "This book about Jija",
                YearOfPublishing = 1974,
                PlaceOfPublication = "Engels",
                PublishingHouse = "Some",
                ISBN = "ISBN 7-4345-3543-4",
                NumberOfPages = 5,
                Authors = new List<Person>() { new Person() {FirstName = "Roman", LastName = "Lezin" } }
            };
            _bookLogic = DependencyResolver.NinjectKernel.Get<IBookLogic>();
            _response = new Response();
        }

        [Test]
        public void BookAdding_CorrectBook_Correct()
        {
           _bookLogic.AddBook(_correctBook, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void BookAdding_CorrectBookWithExistedISBNInCollection_Incorrect()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            Book newBook = new Book()
            {
                Name = "Jiaja",
                Note = "This book aasdbout Jija",
                YearOfPublishing = 1974,
                PublishingHouse = "Some",
                PlaceOfPublication = "Saratov",
                ISBN = "ISBN 7-4345-3543-4",
                NumberOfPages = 6,
                Authors = new List<Person>() { new Person() { Id = 2, FirstName = "Roman", LastName = "Lezin" } }
            };
            _bookLogic.AddBook(newBook, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "Book with this ISBN or name, authors and year of publishing is already exist");
        }

        [Test]
        public void BookAdding_CorrectBookWithExistedNameAuthorAndYear_Incorrect()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            Book newBook = new Book()
            {
                Name = "Jija",
                Note = "This book aasdbout Jija",
                YearOfPublishing = 1974,
                PublishingHouse = "Some",
                PlaceOfPublication = "Saratov",
                ISBN = "ISBN 7-4345-3243-4",
                NumberOfPages = 6,
                Authors = new List<Person>() { new Person() { Id = 2, FirstName = "Ivan", LastName = "Ivanov" } }
            };
            _bookLogic.AddBook(newBook, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "Book with this ISBN or name, authors and year of publishing is already exist");
        }

        [Test]
        public void BookDeleting_NoteWithIdExistInStorage_Correct()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            _bookLogic.DeleteBook(1, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void BookDeleting_NoteWithIdIsNotExistInStorage_Incorrect()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            _bookLogic.DeleteBook(2, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "There is no data with the specified id in the data storage");
        }

        [Test]
        public void BookGettingById_NoteWithIdExistInStorage_Correct()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            var gettedBook = _bookLogic.GetBookById(1);
            Assert.IsNotNull(gettedBook);
        }

        [Test]
        public void BookGettingById_NoteWithIdIsNotExistInStorage_Incorrect()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            var gettedBook = _bookLogic.GetBookById(2);
            Assert.IsNull(gettedBook);
        }

        [Test]
        public void GettingAllBooks_BooksExistsInStorage_Correct()
        {
            var gettedBooks = _bookLogic.GetAllBooks();
            Assert.Greater(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingBooksByAuthor_BookWithEnteredAuthorIsNotExistInStorage_Correct()
        {
            var author = new Person() { Id = 2, FirstName = "Ivan", LastName = "Ivanov" };
            var gettedBooks = _bookLogic.GetBooksByAuthor(author);
            Assert.AreEqual(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingBooksByAuthor_BooksWithEnteredAuthorExistsInStorage_Correct()
        {
            var author = new Person() { Id = 2, FirstName = "Ivan", LastName = "Ivanov" };
            _bookLogic.AddBook(_correctBook, ref _response);
            var gettedBooks = _bookLogic.GetBooksByAuthor(author);
            Assert.Greater(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingBooksByCharSet_BooksWithEnteredCharsExistsInStorage_Correct()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            var gettedBooks = _bookLogic.GetBooksByCharacterSet("Ji");
            Assert.Greater(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingBooksByCharSet_BooksWithEnteredCharsNotExistsInStorage_Correct()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            var gettedBooks = _bookLogic.GetBooksByCharacterSet("As");
            Assert.AreEqual(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingOrderedBooks_BooksExistsInStorage_Correct()
        {
            var newBook = new Book()
            {
                Name = "Jiaja",
                Note = "This book aasdbout Jija",
                YearOfPublishing = 1974,
                PublishingHouse = "Some",
                PlaceOfPublication = "Saratov",
                ISBN = "ISBN 7-4345-3143-4",
                NumberOfPages = 6,
                Authors = new List<Person>() { new Person() { Id = 2, FirstName = "John", LastName = "Bohn" } }
            };
            _bookLogic.AddBook(_correctBook, ref _response);
            _bookLogic.AddBook(newBook, ref _response);
            var gettedBooks = _bookLogic.GetOrderedByBooks();
            Assert.Greater(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingOrderedBooks_BooksIsNotExistsInStorage_Correct()
        {
            var gettedBooks = _bookLogic.GetOrderedByBooks();
            Assert.AreEqual(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingOrderedByDescBooks_BooksExistsInStorage_Correct()
        {
            var newBook = new Book()
            {
                Name = "Jiaja",
                Note = "This book aasdbout Jija",
                YearOfPublishing = 1974,
                PublishingHouse = "Some",
                PlaceOfPublication = "Saratov",
                ISBN = "ISBN 7-4345-3143-4",
                NumberOfPages = 6,
                Authors = new List<Person>() { new Person() { Id = 2, FirstName = "John", LastName = "Bohn" } }
            };
            _bookLogic.AddBook(_correctBook, ref _response);
            _bookLogic.AddBook(newBook, ref _response);
            var gettedBooks = _bookLogic.GetOrderedByDescBooks();
            Assert.Greater(gettedBooks.Count(), 0);
        }

        [Test]
        public void GettingOrderedByDescBooks_BooksIsNotExistsInStorage_Correct()
        {
            var gettedBooks = _bookLogic.GetOrderedByDescBooks();
            Assert.AreEqual(gettedBooks.Count(), 0);
        }
    }
}