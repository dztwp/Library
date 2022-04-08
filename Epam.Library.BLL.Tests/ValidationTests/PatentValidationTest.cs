using Epam.Library.Entities;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.BLL.Tests.ValidationTests
{
    public class PatentValidationTest
    {
        private IValidator<Patent> _patentValidator;
        private Patent _correctPatent;

        [SetUp]
        public void Setup()
        {
            _patentValidator = new Validation.PatentValidator();
            _correctPatent = new Patent()
            {
                Id = 1,
                Name = "Jija",
                Note = "This book about Jija",
                YearOfPublishing = 1974,
                ApplicationDate = 1970,
                Country = "Russia",
                RegistrationNumber = "123456789",
                Authors = new List<Person>() { new Person() { Id = 2, FirstName = "Ivan", LastName = "Ivanov" } },
                NumberOfPages = 5
            };
        }

        [Test]
        public void AllCorrectParams_Correct()
        {
            Patent patent = _correctPatent;
            var result = _patentValidator.Validate(patent);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void NameIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Name = "";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Name must be filled");
        }

        [Test]
        public void NameLegthGreaterThan300AndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Name = new string('f', 301);
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Name can't be greater than 300");
        }

        [Test]
        public void NoteIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Note = "";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Note must be filled");
        }

        [Test]
        public void NoteGreaterThan2000AndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Note = new string('a', 2001);
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Note mustn't be greater than 2000 symbols");
        }

        [Test]
        public void YearOfPublishigLessThan1474AndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.YearOfPublishing = 1469;
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Year Of Publishing must be between 1474 and this year");
        }

        [Test]
        public void YearOfPublishigGreaterThanNowDateAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.YearOfPublishing = DateTime.Now.Year + 1;
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Year Of Publishing must be between 1474 and this year");
        }

        [Test]
        public void ApplicationDateLessThan1474AndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.ApplicationDate = 1469;
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Application Date must be between 1474 and this year");
        }

        [Test]
        public void ApplicationDateGreaterThanNowDateAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.ApplicationDate = DateTime.Now.Year + 1;
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Application Date must be between 1474 and this year");
        }

        [Test]
        public void CountryFieldIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Country = "";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Country must be filled");
        }

        [Test]
        public void CountryStartsWithLowercaseEmptyAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Country = "russia";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Country name must start with capital symbol. It can be written as an abbreviation. Must contain only cyrillic or latin symbols");
        }

        [Test]
        public void CountryFieldHasOneSpaceBetweenWordsAndOtherParamsCorrect_Correct()
        {
            Patent patent = _correctPatent;
            patent.Country = "United States";
            var result = _patentValidator.Validate(patent);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void CountryFieldDontHaveSpacesInTheEndAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Country = "USA ";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Country name must start with capital symbol. It can be written as an abbreviation. Must contain only cyrillic or latin symbols");
        }

        [Test]
        public void CountryFieldStartsWithSpaceAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Country = " USA";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Country name must start with capital symbol. It can be written as an abbreviation. Must contain only cyrillic or latin symbols");
        }

        [Test]
        public void CountryLengthGreaterThan200AndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.Country = new string('a', 201);
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Country can't be greater than 200");
        }

        [Test]
        public void RegistrationNumberNotEqual9AndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.RegistrationNumber = "23434645645";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Registration Number must contains from 1 to 9 digits");
        }

        [Test]
        public void RegistrationNumberContainsCharsAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.RegistrationNumber = "234346a45645";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Registration Number must contains from 1 to 9 digits");
        }

        [Test]
        public void RegistrationNumberEmptyAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.RegistrationNumber = "";
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Registration Number must be filled");
        }

        [Test]
        public void NumberOfPagesLessThanZeroAndOtherParamsCorrect_Incorrect()
        {
            Patent patent = _correctPatent;
            patent.NumberOfPages = 0;
            var result = _patentValidator.Validate(patent);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Number of pages must be positive");
        }

    }
}
