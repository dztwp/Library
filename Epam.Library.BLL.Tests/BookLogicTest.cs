using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using NUnit.Framework;
using Moq;
using Epam.Library.BLL.Interfaces;
using System;
using Epam.Library.DAL.Interfaces;
using System.Collections.Generic;

namespace Epam.Library.BLL.Tests
{
    public class BookLogicTest
    {
        private Response _response;
        private Book _correctBook;
        private Book _inCorrectBook;
        private Person _author;

        [SetUp]
        public void Setup()
        {
            _response = new Response();
            _author = new Person() { Id = 2, FirstName = "Roman", LastName = "Lezin" };
            _correctBook = new Book()
            {
                Id = 1,
                Name = "Biba",
                Note = "asd",
                YearOfPublishing = 1600,
                PlaceOfPublication = "Novgorod",
                PublishingHouse = "Vad",
                ISBN = "ISBN 7-5555-2321-4",
                NumberOfPages = 5,
                Authors = new List<Person> { _author }
            };
            _inCorrectBook = new Book()
            {
                Id = 1,
                Name = "Biba",
                Note = "asd",
                YearOfPublishing = 1600,
                PlaceOfPublication = "aNovgorod",
                PublishingHouse = "Vad",
                ISBN = "ISBN 7-5555-2321-4",
                NumberOfPages = 5,
                Authors = new List<Person> { _author }
            };
        }

        delegate void AddingCallback(Book book, ref Response response);

        [Test]
        public void AddingBook_NewCorrectBook_Correct()
        {
            var bookValidator = new Validation.BooksValidator();
            var bookDAO = new Mock<IBookDao>();
            var personLogic = new Mock<IPersonLogic>();

            personLogic.Setup(personLog => personLog.AddPerson(_author, ref It.Ref<Response>.IsAny)).Returns(_author);
            bookDAO.Setup(obj => obj.AddBook(_correctBook, ref It.Ref<Response>.IsAny)).Returns(_correctBook)
                .Callback(new AddingCallback((Book x, ref Response y) => y.IsSuccess = true));
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            bookLogic.AddBook(_correctBook, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void AddingBook_NewInCorrectBook_Incorrect()
        {
            var bookDAO = new Mock<IBookDao>();
            var personLogic = new Mock<IPersonLogic>();
            var bookValidator = new Validation.BooksValidator();
            bookDAO.Setup(obj => obj.AddBook(_inCorrectBook, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Book x, ref Response y) => y.IsSuccess = true));
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            bookLogic.AddBook(_inCorrectBook, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        delegate void DeleteCallback(int id, ref Response response);

        [Test]
        public void DeletingBook_DataWithIdIsExist_Correct()
        {
            int id = 1;
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();
            bookDAO
                .Setup(obj => obj.DeleteBook(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) => y.IsSuccess = true));
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            bookLogic.DeleteBook(id, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void DeletingBook_DataWithIdIsNotExist_Incorrect()
        {
            int id = 1;
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO.Setup(obj => obj.DeleteBook(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("There is no data with the specified id in the data storage"));
                    y.IsSuccess = false;
                }
                ));
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            bookLogic.DeleteBook(id, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void GettingAllBooks_BooksExistsInStorage_Correct()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetAllBooks())
                .Returns(new List<Book>() { _inCorrectBook });
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetAllBooks();
            Assert.Greater(bookCollection.Count, 0);
        }

        [Test]
        public void GettingAllBooks_BooksIsNotExistsInStorage_Incorrect()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetAllBooks())
                .Returns(new List<Book>());
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetAllBooks();
            Assert.AreEqual(bookCollection.Count, 0);
        }

        [Test]
        public void GettingBooksByAuthor_AuthorsBookExistInStorage_Correct()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetBooksByAuthor(_author))
                .Returns(new List<Book>() { _correctBook });
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetBooksByAuthor(_author);
            Assert.Greater(bookCollection.Count, 0);
        }

        [Test]
        public void GettingBooksByAuthor_AuthorsBookIsNotExistInStorage_Incorrect()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetBooksByAuthor(_author))
                .Returns(new List<Book>());
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetBooksByAuthor(_author);
            Assert.AreEqual(bookCollection.Count, 0);
        }

        [Test]
        public void GettingBookById_BookWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            int id = 1;
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetBookById(id))
                .Returns((Book)null);
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var book = bookLogic.GetBookById(id);
            Assert.AreEqual(book, null);
        }

        [Test]
        public void GettingBookById_BookWithEnteredExistInStorage_Correct()
        {
            int id = 1;
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetBookById(id))
                .Returns(_correctBook);
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var book = bookLogic.GetBookById(id);
            Assert.IsNotNull(book);
        }

        [Test]
        public void GettingBookByCharacterSet_BooksWithNameStartedOnCharacterSetIsExist_Correct()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetBooksByCharacterSet("Bi"))
                .Returns(new List<Book>() { _correctBook });
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetBooksByCharacterSet("Bi");
            Assert.Greater(bookCollection.Count, 0);
        }

        [Test]
        public void GettingBookByCharacterSet_BooksWithNameStartedOnCharacterSetIsNull_Correct()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetBooksByCharacterSet(null))
                .Returns((IEnumerable<Book>)null);
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetBooksByCharacterSet(null);
            Assert.AreEqual(bookCollection, null);
        }

        [Test]
        public void GettingOrderedBooks_BooksExistInStorage_Correct()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetOrderedByBooks())
                .Returns(new List<Book>() { _correctBook });
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetOrderedByBooks();
            Assert.Greater(bookCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedBooks_BooksIsNotExistInStorage_Incorrect()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetOrderedByBooks())
                .Returns(new List<Book>());
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetOrderedByBooks();
            Assert.AreEqual(bookCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedByDescBooks_BooksExistInStorage_Correct()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetOrderedByDescBooks())
                .Returns(new List<Book>() { _correctBook });
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetOrderedByDescBooks();
            Assert.Greater(bookCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedByDescBooks_BooksIsNotExistInStorage_Incorrect()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO
                .Setup(obj => obj.GetOrderedByDescBooks())
                .Returns(new List<Book>());
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            var bookCollection = (List<Book>)bookLogic.GetOrderedByDescBooks();
            Assert.AreEqual(bookCollection.Count, 0);
        }

        delegate void UpdatingCallback(Book book, ref Response response);

        [Test]
        public void UpdatingBook_NewCorrectBook_Correct()
        {
            var bookValidator = new Validation.BooksValidator();
            var bookDAO = new Mock<IBookDao>();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO.Setup(obj => obj.UpdateBook(_correctBook, ref It.Ref<Response>.IsAny))
                .Callback(new UpdatingCallback((Book x, ref Response y) => y.IsSuccess = true));
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            bookLogic.UpdateBook(_correctBook, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void UpdatingBook_NewInCorrectBook_Incorrect()
        {
            var bookDAO = new Mock<IBookDao>();
            var bookValidator = new Validation.BooksValidator();
            var personLogic = new Mock<IPersonLogic>();

            bookDAO.Setup(obj => obj.UpdateBook(_inCorrectBook, ref It.Ref<Response>.IsAny))
                .Callback(new UpdatingCallback((Book x, ref Response y) => y.IsSuccess = true));
            IBookLogic bookLogic = new BookLogic(bookDAO.Object, personLogic.Object, bookValidator);
            bookLogic.UpdateBook(_inCorrectBook, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }
    }
}