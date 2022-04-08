using Epam.Library.Entities;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.BLL.Tests.ValidationTests
{
    public class BookValidationTests
    {
        private IValidator<Book> _bookValidator;
        private Book _correctBook;
        private readonly string _cityErrorText = "Place Of Publication must contains Russian or Latin characters starting with a capital letter. May contain a hyphen, in this case the first character after the hyphen is capitalized. It can contain a double hyphen, in which case the character becomes capitalized only after the second hyphen. The hyphen cannot be the first or last character. May contain spaces. The space can be followed by either a lowercase or an uppercase letter.";

        [SetUp]
        public void Setup()
        {
            _correctBook = new Book
            {
                Id = 1,
                Name = "Jija",
                Note = "This book about Jija",
                YearOfPublishing = 1974,
                PlaceOfPublication = "Engels",
                PublishingHouse = "Some",
                ISBN = "ISBN 7-4345-3543-4",
                NumberOfPages = 5,
                Authors = new List<Person>() { new Person() { Id = 2, FirstName = "Ivan", LastName = "Ivanov" } }
            };
            _bookValidator = new Validation.BooksValidator();
        }

        [Test]
        public void AllCorrectParams_Correct()
        {
            Book book = _correctBook;
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void NameIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.Name = "";
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Name must be filled");
        }

        [Test]
        public void NameLegthGreaterThan300AndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.Name = new string('a', 301);
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Name can't be greater than 300");
        }

        [Test]
        public void NoteIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.Note = "";
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Note must be filled");
        }

        [Test]
        public void NoteGreaterThan2000AndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.Note = new string('a', 2001);
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Note mustn't be greater than 2000 symbols");
        }

        [Test]
        public void YearOfPublishigLessThan1400AndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
        book.YearOfPublishing = 1350;
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Year Of Publishing must be between 1400 and this year");
        }

        [Test]
        public void YearOfPublishigGreaterThanNowDateAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.YearOfPublishing = DateTime.Now.Year + 1;
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Year Of Publishing must be between 1400 and this year");
        }

        [Test]
        public void PlaceOfPublicationIsEmptyeAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "";
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Place Of Publication must be filled");
        }

        [Test]
        public void PlaceOfPublicationStartAtLowercaseAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "new-York";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationStartWithSpaceAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = " New-York";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationEndWithSpaceAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "New-York ";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationStartWithHypherAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "-New-York";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationEndWithHypherAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "New-";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationHaveCorrectDoubledHypherAndOtherParamsCorrect_Correct()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "Rostov-on-Don";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void PlaceOfPublicationHaveDoubledHypherThatStratWithUpperCaseAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "Rostov-On-Don";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationHaveDoubledHypherThatIsEndAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PlaceOfPublication = "Rostov-On-";
            var result = _bookValidator.Validate(book);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PublishingHouseIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PublishingHouse = "";
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Publishing House must be filled");
        }

        [Test]
        public void PublishingHouseLegthGreaterThan300AndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.PublishingHouse = new string('a', 301);
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Publishing House can't be greater than 300");
        }

        [Test]
        public void ISBNHasIncorrectFormAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.ISBN = "sdfsgdfga";
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "ISBN must be introduced according to the standart");
        }

        [Test]
        public void NumberOfPagesLessThanOneAndOtherParamsCorrect_Incorrect()
        {
            Book book = _correctBook;
            book.NumberOfPages = 0;
            var result = _bookValidator.Validate(book);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Number of pages must be positive");
        }
    }
}


