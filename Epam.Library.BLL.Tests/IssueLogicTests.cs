using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using Moq;
using NUnit.Framework;
using System;

namespace Epam.Library.BLL.Tests
{
    public class IssueLogicTests
    {
        private Response _response;
        private Issue _correctIssue;
        private Issue _incorrectIssue;


        [SetUp]
        public void Setup()
        {
            _response = new Response();
            _correctIssue = new Issue()
            {
                Id = 1,
                NumberOfIssue = 1,
                NumberOfPages = 5,
                ReleaseDay = 1650
            };
            _incorrectIssue = new Issue()
            {
                Id = 1,
                NumberOfIssue = 1,
                NumberOfPages = -15,
                ReleaseDay = 1650
            };
        }

        delegate void DeleteCallback(int id, ref Response response);

        [Test]
        public void DeletingBook_DataWithIdIsExist_Correct()
        {
            int id = 1;
            var issueDAO = new Mock<IIssueDao>();
            var issueValidator = new Validation.IssueValidator();
            issueDAO
                .Setup(obj => obj.DeleteIssue(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) => y.IsSuccess = true));
            IIssueLogic issueLogic = new IssueLogic(issueDAO.Object, issueValidator);
            issueLogic.DeleteIssue(id, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void DeletingIssue_DataWithIdIsNotExist_Incorrect()
        {
            int id = 1;
            var issueDAO = new Mock<IIssueDao>();
            var issueValidator = new Validation.IssueValidator();
            issueDAO
                .Setup(obj => obj.DeleteIssue(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("There is no data with the specified id in the data storage"));
                    y.IsSuccess = false;
                }
                ));
            IIssueLogic issueLogic = new IssueLogic(issueDAO.Object, issueValidator);
            issueLogic.DeleteIssue(id, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void GettingIssueById_IssueWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            int id = 1;
            var issueDAO = new Mock<IIssueDao>();
            var issueValidator = new Validation.IssueValidator();
            issueDAO
                .Setup(obj => obj.GetIssueById(id))
                .Returns((Issue)null);
            IIssueLogic issueLogic = new IssueLogic(issueDAO.Object, issueValidator);
            var issue = issueLogic.GetIssueById(id);
            Assert.AreEqual(issue, null);
        }

        [Test]
        public void GettingIssueById_IssueWithEnteredExistInStorage_Correct()
        {
            int id = 1;
            var issueDAO = new Mock<IIssueDao>();
            var issueValidator = new Validation.IssueValidator();
            issueDAO
                .Setup(obj => obj.GetIssueById(id))
                .Returns(_correctIssue);
            IIssueLogic issueLogic = new IssueLogic(issueDAO.Object, issueValidator);
            var issue = issueLogic.GetIssueById(id);
            Assert.IsNotNull(issue);
        }

        delegate void UpdateCallback(Issue issue, ref Response response);

        [Test]
        public void UpdatingIssue_CorrectIssueAndNewspaper_Correct()
        {
            var issueValidator = new Validation.IssueValidator();
            var issueDAO = new Mock<IIssueDao>();
            issueDAO.Setup(obj => obj.UpdateIssue(_correctIssue, ref It.Ref<Response>.IsAny))
                .Callback(new UpdateCallback((Issue x, ref Response y) => y.IsSuccess = true));
            IIssueLogic issueLogic = new IssueLogic(issueDAO.Object, issueValidator);
            issueLogic.UpdateIssue(_correctIssue, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void UpdatingIssue_InCorrectIssue_Incorrect()
        {
            var issueValidator = new Validation.IssueValidator();
            var issueDAO = new Mock<IIssueDao>();
            issueDAO.Setup(obj => obj.UpdateIssue(_incorrectIssue, ref It.Ref<Response>.IsAny))
                .Callback(new UpdateCallback((Issue x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("The specified newspaper does not exist in the data storage"));
                    y.IsSuccess = false;
                }));
            IIssueLogic issueLogic = new IssueLogic(issueDAO.Object, issueValidator);
            issueLogic.UpdateIssue(_incorrectIssue, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

    }
}

