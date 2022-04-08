using Epam.Library.BLL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using NUnit.Framework;
using System;
using Epam.Library.Dependencies;
using Ninject;
using System.Linq;
using System.Collections.Generic;
using Epam.Library.DAL.FakeDAL;

namespace Epam.Library.BLL.IntegrationTests
{   
    public class IssueIntegrationTests
    {
        private Response _response;
        private Issue _correctIssue;
        private Newspaper _correctNewspaper;
        private IIssueLogic _issueLogic;
        private INewspaperLogic _newspaperLogic;

        [SetUp]
        public void Setup()
        {
            DataStorage.IssueStorage.Clear();
            _response = new Response();
            _correctIssue = new Issue()
            {
                NumberOfIssue = 1,
                NumberOfPages = 5,
                ReleaseDay = 1750
            };
            _correctNewspaper = new Newspaper()
            {
                Id = 2,
                ISSN = "ISSN 0317-8471",
                CollectionOfIssues = new List<Issue>() { _correctIssue },
                Name = "Times",
                Note = "This newspaper is Times",
                PlaceOfPublication = "New-York",
                PublishingHouse = "Kfc",
                YearOfPublishing = 1649
            };
            _issueLogic = DependencyResolver.NinjectKernel.Get<IIssueLogic>();
            _newspaperLogic = DependencyResolver.NinjectKernel.Get<INewspaperLogic>();
        }

        [Test]
        public void AddingIssueToCollection_CorrectIssue_Correct()
        {
            var issue = _issueLogic.AddIssue(_correctIssue, ref _response);
            Assert.Greater(issue.Id, 0);
        }

        [Test]
        public void DeletingIssue_IdExistInCollection_Correct()
        {
            _newspaperLogic.AddNewspaper(_correctNewspaper, ref _response);
            var issue = _issueLogic.AddIssue(_correctIssue, ref _response);
            _issueLogic.DeleteIssue(issue.Id, ref _response);
            Assert.True(_response.IsSuccess);
        }

        [Test]
        public void DeletingIssue_IdIsNotExistInCollection_InCorrect()
        {
            _issueLogic.AddIssue(_correctIssue, ref _response);
            _issueLogic.DeleteIssue(456, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "The specified newspaper does not exist in the data storage");
        }

        [Test]
        public void GettingIssue_IdExistInCollection_Correct()
        {
            var issue = _issueLogic.AddIssue(_correctIssue, ref _response);
            _issueLogic.GetIssueById(issue.Id);
            Assert.True(_response.IsSuccess);
        }

        [Test]
        public void GettingIssue_IdIsNotExistInCollection_InCorrect()
        {
            _issueLogic.AddIssue(_correctIssue, ref _response);
            var gettedIssue = _issueLogic.GetIssueById(2);
            Assert.IsNull(gettedIssue);
        }


    }
}
