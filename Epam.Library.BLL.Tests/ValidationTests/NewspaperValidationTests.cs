using Epam.Library.Entities;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.BLL.Tests.ValidationTests
{
    public class NewspaperValidationTests
    {
        private IValidator<Newspaper> _newspaperValidator;
        private Newspaper _correctNewspaper;
        private readonly string _cityErrorText = "Place Of Publication must contains Russian or Latin characters starting with a capital letter. May contain a hyphen, in this case the first character after the hyphen is capitalized. It can contain a double hyphen, in which case the character becomes capitalized only after the second hyphen. The hyphen cannot be the first or last character. May contain spaces. The space can be followed by either a lowercase or an uppercase letter.";

        [SetUp]
        public void Setup()
        {
            _newspaperValidator = new Validation.NewspapersValidator();
            _correctNewspaper = new Newspaper()
            {
                Id = 1,
                Name = "Jija",
                Note = "This book about Jija",
                YearOfPublishing = 1974,
                PlaceOfPublication = "Engels",
                PublishingHouse = "Some",
                ISSN = "ISSN 4556-3312",
                CollectionOfIssues = new List<Issue>()
            };
        }

        [Test]
        public void AllCorrectParams_Correct()
        {
            Newspaper newspaper = _correctNewspaper;
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void NameIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.Name = "";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Name must be filled");
        }

        [Test]
        public void NameLegthGreaterThan300AndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.Name = new string('a', 301);
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Name can't be greater than 300");
        }

        [Test]
        public void NoteIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.Note = "";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Note must be filled");
        }

        [Test]
        public void NoteGreaterThan2000AndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.Note = new string('a', 2001);
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Note mustn't be greater than 2000 symbols");
        }

        [Test]
        public void YearOfPublishigLessThan1400AndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.YearOfPublishing = 1350;
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Year Of Publishing must be between 1400 and this year");
        }

        [Test]
        public void YearOfPublishigGreaterThanNowDateAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.YearOfPublishing = DateTime.Now.Year + 1;
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Year Of Publishing must be between 1400 and this year");
        }

        [Test]
        public void PlaceOfPublicationIsEmptyeAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Place Of Publication must be filled");
        }

        [Test]
        public void PlaceOfPublicationStartAtLowercaseAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "new-York";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationStartWithSpaceAndOtherParamsCorrect_Incorrect()
        {

            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = " New-York";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationEndWithSpaceAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "New-York ";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationStartWithHypherAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "-New-York";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationEndWithHypherAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "New-";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationHaveCorrectDoubledHypherAndOtherParamsCorrect_Correct()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "Rostov-on-Don";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.Count(), 0);
        }

        [Test]
        public void PlaceOfPublicationHaveDoubledHypherThatStratWithUpperCaseAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "Rostov-On-Don";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PlaceOfPublicationHaveDoubledHypherThatIsEndAndOtherParamsCorrect_Incorrect()
        {

            Newspaper newspaper = _correctNewspaper;
            newspaper.PlaceOfPublication = "Rostov-On-";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.AreEqual(result.Errors.FirstOrDefault().ErrorMessage, _cityErrorText);
        }

        [Test]
        public void PublishingHouseIsEmptyAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PublishingHouse = "";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Publishing House must be filled");
        }

        [Test]
        public void PublishingHouseLegthGreaterThan300AndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.PublishingHouse = new string('a', 301);
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Length of Publishing House can't be greater than 300");
        }

        [Test]
        public void ISSNHasIncorrectFormAndOtherParamsCorrect_Incorrect()
        {
            Newspaper newspaper = _correctNewspaper;
            newspaper.ISSN = "asdwwrwe";
            var result = _newspaperValidator.Validate(newspaper);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "ISSN must be introduced according to the standart");
        }


    }
}
