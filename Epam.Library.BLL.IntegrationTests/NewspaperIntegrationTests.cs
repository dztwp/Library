using Epam.Library.BLL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Epam.Library.Dependencies;
using Ninject;
using System.Linq;
using Epam.Library.DAL.FakeDAL;

namespace Epam.Library.BLL.IntegrationTests
{
    public class NewspaperIntegrationTests
    {
        private Newspaper _correctNewspaper;
        private Issue _correctIssue;
        private INewspaperLogic _newspaperLogic;
        private Response _response;

        [SetUp]
        public void Setup()
        {
            DataStorage.Storage.Clear();
            DataStorage.IssueStorage.Clear();
            _correctNewspaper = new Newspaper()
            {
                ISSN = "ISSN 0317-8471",
                CollectionOfIssues = new List<Issue>(),
                Name = "Times",
                Note = "This newspaper is Times",
                PlaceOfPublication = "New-York",
                PublishingHouse = "Kfc",
                YearOfPublishing = 1649
            };
            _correctIssue = new Issue()
            {
                Id = 1,
                NumberOfIssue = 1,
                NumberOfPages = 5,
                ReleaseDay = 1750
            };
            _response = new Response();
            _newspaperLogic = DependencyResolver.NinjectKernel.Get<INewspaperLogic>();
        }

        [Test]
        public void NewspaperAdding_CorrectNewspaper_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            Assert.True(_response.IsSuccess);
        }

        [Test]
        public void NewspaperAdding_TryToAddAlreadyExistedNewspaper_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "Book with this name, publishing house and year of publishing is already exist");
        }

        [Test]
        public void NewspaperAdding_TryToAddNewspapersWithSameISSNButDifferentNames_Incorrect()
        {
            var newNewspaper = new Newspaper()
            {
                ISSN = "ISSN 0317-8471",
                CollectionOfIssues = new List<Issue>(),
                Name = "Komapravda",
                Note = "This newspaper is Times",
                PlaceOfPublication = "New-York",
                PublishingHouse = "Kfc",
                YearOfPublishing = 1649
            };
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            _newspaperLogic.AddNewspaper(newNewspaper, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "Newspaper with this ISSN already exist, but the names don't match");
        }

        [Test]
        public void IssueAddingToNewspaper_CorrectIssueAndNewspaper_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperWithNewIssue = _newspaperLogic.AddIssueToNewspaper(_correctIssue, _correctNewspaper, ref _response);
            Assert.IsNotNull(newspaperWithNewIssue.CollectionOfIssues.FirstOrDefault(issue => issue.Id == _correctIssue.Id));
        }

        [Test]
        public void IssueAddingToNewspaper_IssueDateLessThanNewspaperYearOfPublishing_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            _correctIssue.ReleaseDay = 1648;
            var newspaperWithNewIssue = _newspaperLogic.AddIssueToNewspaper(_correctIssue, _correctNewspaper, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "Issues date of release must be greater than newspaper year of publishing"); ;
        }

        [Test]
        public void IssueAddingToNewspaper_IssueAlreadyExistInNewspaperList_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            _newspaperLogic.AddIssueToNewspaper(_correctIssue, _correctNewspaper, ref _response);
            _newspaperLogic.AddIssueToNewspaper(_correctIssue, _correctNewspaper, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "That issue already exist in choosed newspaper"); ;
        }

        [Test]
        public void NewspaperDeleting_NewspaperWithEnteredIdExistInStorage_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            _newspaperLogic.DeleteNewspaper(1, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void NewspaperDeleting_NewspaperWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            _newspaperLogic.DeleteNewspaper(2, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "There is no data with the specified id in the data storage");
        }

        [Test]
        public void GettingAllNewspapers_NewspapersExistInStorage_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetAllNewspapers();
            Assert.Greater(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingAllNewspapers_NewspapersIsNotExistInStorage_Correct()
        {
            var newspaperCollection = _newspaperLogic.GetAllNewspapers();
            Assert.AreEqual(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingNewspapersByPublishingHouse_PublishingHouseIsCorrect_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetNewspapersByPublishingHouse("Kfc");
            Assert.Greater(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingNewspapersByPublishingHouse_PublishingHouseIsNotExistInStorage_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetNewspapersByPublishingHouse("Kfcasad");
            Assert.AreEqual(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingNewspapersByPublishingHouse_PublishingHouseIsNull_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetNewspapersByPublishingHouse(null);
            Assert.IsNull(newspaperCollection);
        }

        [Test]
        public void GettingNewspaperById_NoteWithEnteredIdExistInStorage_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaper = _newspaperLogic.GetNewspaperById(1);
            Assert.IsNotNull(newspaper);
        }

        [Test]
        public void GettingNewspaperById_NoteWithEnteredIdIsNotExistInStorage_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaper = _newspaperLogic.GetNewspaperById(2);
            Assert.IsNull(newspaper);
        }

        [Test]
        public void GettingNewspapersByCharSet_CharSetIsCorrect_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetNewspapersByCharacterSet("Ti");
            Assert.Greater(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingNewspapersByCharSet_CharSetIsNull_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetNewspapersByCharacterSet(null);
            Assert.IsNull(newspaperCollection);
        }

        [Test]
        public void GettingNewspapersByCharSet_CharSetIsIncorrect_Incorrect()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetNewspapersByCharacterSet("asd");
            Assert.AreEqual(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedNewspapers_StorageIsNotEmpty_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetOrderedByNewspapers();
            Assert.Greater(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedNewspapers_StorageIsEmpty_Correct()
        {
            var newspaperCollection = _newspaperLogic.GetOrderedByNewspapers();
            Assert.AreEqual(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedByDescNewspapers_StorageIsNotEmpty_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var newspaperCollection = _newspaperLogic.GetOrderedByDescNewspapers();
            Assert.Greater(newspaperCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedByDescNewspapers_StorageIsEmpty_Correct()
        {
            var newspaperCollection = _newspaperLogic.GetOrderedByDescNewspapers();
            Assert.AreEqual(newspaperCollection.Count(), 0);
        }
    }
}
