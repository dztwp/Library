using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Epam.Library.BLL.Tests
{
    public class NewspaperLogicTests
    {
        private Response _response;
        private Newspaper _correctNewspaper;
        private Newspaper _inCorrectNewspaper;

        [SetUp]
        public void Setup()
        {
            _response = new Response();
            _correctNewspaper = new Newspaper()
            {
                Id = 2,
                ISSN = "ISSN 0317-8471",
                CollectionOfIssues = new List<Issue>(),
                Name = "Times",
                Note = "This newspaper is Times",
                PlaceOfPublication = "New-York",
                PublishingHouse = "Kfc",
                YearOfPublishing = 1649
            };
            _inCorrectNewspaper = new Newspaper()
            {
                Id = 2,
                ISSN = "ISSN 0317-8471",
                CollectionOfIssues = new List<Issue>(),
                Name = "Times",
                Note = "This newspaper is Times",
                PlaceOfPublication = "asdgwNew-York",
                PublishingHouse = "Kfc",
                YearOfPublishing = 1649
            };
        }

        delegate void AddingCallback(Newspaper book, ref Response response);

        [Test]
        public void AddingNewspaper_NewCorrectNewspaper_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO.Setup(obj => obj.AddNewspaper(_correctNewspaper, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Newspaper x, ref Response y) => y.IsSuccess = true));
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void AddingNewspaper_NewInCorrectNewspaper_Incorrect()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO.Setup(obj => obj.AddNewspaper(_inCorrectNewspaper, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Newspaper x, ref Response y) => y.IsSuccess = true));
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            newspaperLogic.AddNewspaper(_inCorrectNewspaper, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }



        delegate void AddingCallbackForIssues(Issue issue, Newspaper newspaper, ref Response response);

        [Test]
        public void AddingIssue_CorrectIssueAndNewspaper_Correct()
        {
            Issue newIssue = new Issue
            {
                Id = 1,
                NumberOfIssue = 1,
                NumberOfPages = 5,
                ReleaseDay = 1700
            };
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO.Setup(obj => obj.AddIssueToNewspaper(newIssue, _correctNewspaper, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallbackForIssues((Issue x, Newspaper z, ref Response y) => y.IsSuccess = true));
            INewspaperLogic issueLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            issueLogic.AddIssueToNewspaper(newIssue, _correctNewspaper, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void AddingIssue_InCorrectIssue_Incorrect()
        {
            Issue newIssue = new Issue
            {
                Id = 1,
                NumberOfIssue = 1,
                NumberOfPages = 5,
                ReleaseDay = 1700
            };
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO.Setup(obj => obj.AddIssueToNewspaper(newIssue, _correctNewspaper, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallbackForIssues((Issue x, Newspaper z, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("The specified newspaper does not exist in the data storage"));
                    y.IsSuccess = false;
                }));
            INewspaperLogic issueLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            issueLogic.AddIssueToNewspaper(newIssue, _correctNewspaper, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        delegate void DeleteCallback(int id, ref Response response);

        [Test]
        public void DeletingNewspaper_DataWithIdIsExist_Correct()
        {
            int id = 1;
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.DeleteNewspaper(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) => y.IsSuccess = true));
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            newspaperLogic.DeleteNewspaper(id, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void DeletingNewspaper_DataWithIdIsNotExist_Incorrect()
        {
            int id = 1;
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.DeleteNewspaper(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("There is no data with the specified id in the data storage"));
                    y.IsSuccess = false;
                }
                ));
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            newspaperLogic.DeleteNewspaper(id, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void GettingAllNewspapers_NewspaperExistsInStorage_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetAllNewspapers())
                .Returns(new List<Newspaper>() { _inCorrectNewspaper });
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperList = (List<Newspaper>)newspaperLogic.GetAllNewspapers();
            Assert.Greater(newspaperList.Count, 0);
        }

        [Test]
        public void GettingAllNewspapers_NewspaperIsNotExistsInStorage_Incorrect()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetAllNewspapers())
                .Returns(new List<Newspaper>());
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetAllNewspapers();
            Assert.AreEqual(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingNewspapersByPublishingHouse_PublishingHouseNewspaperExistInStorage_Correct()
        {
            string publishingHouse = "Kfc";
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetNewspapersByPublishingHouse(publishingHouse))
                .Returns(new List<Newspaper>() { _correctNewspaper });
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetNewspapersByPublishingHouse(publishingHouse);
            Assert.Greater(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingNewspapersByPublishingHouse_PublishingHouseNewspaperIsNotExistInStorage_Incorrect()
        {
            string publishingHouse = "Kfc";
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetNewspapersByPublishingHouse(publishingHouse))
                .Returns(new List<Newspaper>());
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetNewspapersByPublishingHouse(publishingHouse);
            Assert.AreEqual(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingNewspaperById_NewspaperWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            int id = 1;
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetNewspaperById(id))
                .Returns((Newspaper)null);
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaper = newspaperLogic.GetNewspaperById(id);
            Assert.AreEqual(newspaper, null);
        }

        [Test]
        public void GettingNewspaperById_NewspaperWithEnteredIdExistInStorage_Incorrect()
        {
            int id = 1;
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetNewspaperById(id))
                .Returns(_correctNewspaper);
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaper = newspaperLogic.GetNewspaperById(id);
            Assert.IsNotNull(newspaper);
        }

        [Test]
        public void GettingNewspaperByCharacterSet_NewspaperWithNameStartedOnCharacterSetIsExist_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetNewspapersByCharacterSet("Tim"))
                .Returns(new List<Newspaper>() { _correctNewspaper });
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetNewspapersByCharacterSet("Tim");
            Assert.Greater(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingNewspaperByCharacterSet_NewspaperWithNameStartedOnCharacterSetIsNull_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetNewspapersByCharacterSet(null))
                .Returns((IEnumerable<Newspaper>)null);
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetNewspapersByCharacterSet(null);
            Assert.AreEqual(newspaperCollection, null);
        }

        [Test]
        public void GettingOrderedNewspapers_NewspapersExistInStorage_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetOrderedByNewspapers())
                .Returns(new List<Newspaper>() { _correctNewspaper });
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetOrderedByNewspapers();
            Assert.Greater(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedNewspapers_NewspapersIsNotExistInStorage_Incorrect()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetOrderedByNewspapers())
                .Returns(new List<Newspaper>());
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetOrderedByNewspapers();
            Assert.AreEqual(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedByDescNewspapers_NewspapersExistInStorage_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetOrderedByDescNewspapers())
                .Returns(new List<Newspaper>() { _correctNewspaper });
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetOrderedByDescNewspapers();
            Assert.Greater(newspaperCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedByDescNewspapers_NewspapersIsNotExistInStorage_Incorrect()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO
                .Setup(obj => obj.GetOrderedByDescNewspapers())
                .Returns(new List<Newspaper>());
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            var newspaperCollection = (List<Newspaper>)newspaperLogic.GetOrderedByDescNewspapers();
            Assert.AreEqual(newspaperCollection.Count, 0);
        }

        delegate void UpdatingCallback(Newspaper book, ref Response response);

        [Test]
        public void UpdatingNewspaper_NewCorrectNewspaper_Correct()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO.Setup(obj => obj.UpdateNewspaper(_correctNewspaper, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Newspaper x, ref Response y) => y.IsSuccess = true));
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            newspaperLogic.UpdateNewspaper(_correctNewspaper, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void UpdatingNewspaper_NewInCorrectNewspaper_Incorrect()
        {
            var newspaperValidator = new Validation.NewspapersValidator();
            var newspaperDAO = new Mock<INewspaperDao>();
            newspaperDAO.Setup(obj => obj.UpdateNewspaper(_inCorrectNewspaper, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Newspaper x, ref Response y) => y.IsSuccess = true));
            INewspaperLogic newspaperLogic = new NewspaperLogic(newspaperDAO.Object, newspaperValidator);
            newspaperLogic.UpdateNewspaper(_inCorrectNewspaper, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }
    }
}
