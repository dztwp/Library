using Epam.Library.Entities;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Linq;

namespace Epam.Library.BLL.Tests.ValidationTests
{
    public class IssueValidationTests
    {
        private IValidator<Issue> _issueValidator;
        private Issue _correctIssue;

        [SetUp]
        public void Setup()
        {
            _issueValidator = new Validation.IssueValidator();
            _correctIssue = new Issue
            {
                Id = 1,
                NumberOfIssue = 5,
                ReleaseDay = 1700,
                NumberOfPages = 5
            };
        }

        [Test]
        public void AllParamsOfIssueCorrect_Correct()
        {
            Issue issue = _correctIssue;
            var result = _issueValidator.Validate(issue);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [Test]
        public void ReleaseDayLessThan1400AndOtherParamsCorrect_Incorrect()
        {
            Issue issue = _correctIssue;
            issue.ReleaseDay = 1390;
            var result = _issueValidator.Validate(issue);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Release Day must be between 1400 and this year");
        }

        [Test]
        public void ReleaseDayGreaterThanNowDateAndOtherParamsCorrect_Incorrect()
        {
            Issue issue = _correctIssue;
            issue.ReleaseDay = DateTime.Now.Year + 1;
            var result = _issueValidator.Validate(issue);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Release Day must be between 1400 and this year");
        }

        [Test]
        public void NumberOfIssueIsZeroAndOtherParamsCorrect_Incorrect()
        {
            Issue issue = _correctIssue;
            issue.NumberOfIssue = 0;
            var result = _issueValidator.Validate(issue);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Number of issue must be greater than 0");
        }

        [Test]
        public void NumberOfPagesIsZeroAndOtherParamsCorrect_Incorrect()
        {
            Issue issue = _correctIssue;
            issue.NumberOfPages = 0;
            var result = _issueValidator.Validate(issue);
            Assert.IsTrue(result.Errors.FirstOrDefault().ErrorMessage == "Number of pages must be greater than 0");
        }
    }
}
