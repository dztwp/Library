using Epam.Library.Entities;
using FluentValidation;
using NUnit.Framework;
using System.Linq;

namespace Epam.Library.BLL.Tests.ValidationTests
{
    public class AuthorValidationTests
    {
        private string _correctFirstNameOnLatin;
        private string _correctLastNameOnLatin;
        private string _correctFirstNameOnCyrillic;
        private string _correctLastNameOnCyrillic;
        private IValidator<Person> _personValidator;
        private readonly string _firstNameRegularError = "Incorrect input of First Name.Name must contain either Cyrillic or Latin. If the last name is written with a hyphen, the character after the hyphen is capitalized.";
        private readonly string _lastNameRegularError = "Incorrect input of Last Name.Name must contain either Cyrillic or Latin. After the prefix, the surname is capitalized.If the surname is with a hyphen, a capital letter is placed after the hyphen.It can contain an apostrophe and a capital letter is written after the apostrophe.";

        [SetUp]
        public void Setup()
        {
            _correctFirstNameOnLatin = "Roman";
            _correctLastNameOnLatin = "Lezin";
            _correctFirstNameOnCyrillic = "Керил";
            _correctLastNameOnCyrillic = "Иванов";
            _personValidator = new Validation.AuthorValidator();
        }

        [Test]
        public void CorrectFirstAndLastNamesOnLatin_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = _correctLastNameOnLatin };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void CorrectFirstAndLastNamesOnCyrillic_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnCyrillic, LastName = _correctLastNameOnCyrillic };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void EmptyFirstAndCorrectLastNamesOnCyrillic_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = "", LastName = _correctLastNameOnCyrillic };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, "First Name must be filled");
        }

        [Test]
        public void CorrectFirstAndEmptyLastNamesOnCyrillic_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, "Last Name must be filled");
        }

        [Test]
        public void FirstNameStartedWithLowercaseAndCorrectLastNamesOnLatin_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = "roman", LastName = _correctLastNameOnLatin };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _firstNameRegularError);
        }

        [Test]
        public void FirstNameCorrectAndLastNameStartAtLowerCase_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "lezin" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _lastNameRegularError);
        }

        [Test]
        public void FirstNameEndWithHypherAndLastNameIsCorrect_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = "Roman-", LastName = _correctLastNameOnLatin };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _firstNameRegularError);
        }

        [Test]
        public void FirstNameCorrectAndLastNameEndWithHypher_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "Lezin-" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _lastNameRegularError);
        }

        [Test]
        public void FirstNameCorrectAndLastNameStartWithSpace_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = " Lezin" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _lastNameRegularError);
        }

        [Test]
        public void FirstNameStartWithSpaceAndLastNameCorrect_Incorrect()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = " Roman", LastName = _correctLastNameOnLatin };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _firstNameRegularError);
        }

        [Test]
        public void FirstNameCorrectAndLastNameContainsPrefix_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "el Lezini" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void FirstNameCorrectAndLastNameContainsApostrophe_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "d'Lezini" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void FirstNameCorrectAndLastNameContainsPrefixAndApostrophe_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "el d'Lezini" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void FirstNameCorrectAndLastNameContainsHypher_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "Lezini-Ivanov" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void FirstNameCorrectAndLastNameContainsPrefixAndApostropheAndHypher_Correct()
        {
            Person correctPerson = new Person() { Id = 1, FirstName = _correctFirstNameOnLatin, LastName = "el d'Lezini-Ivanov" };
            var result = _personValidator.Validate(correctPerson);
            Assert.AreEqual(result.Errors.Count, 0);
        }

    }
}
