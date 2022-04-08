using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using FluentValidation;
using System;

namespace Epam.Library.BLL
{
    public class IssueLogic : IIssueLogic
    {
        private readonly IIssueDao _issueStorage;
        private readonly IValidator<Issue> _issueValidator;

        public IssueLogic(IIssueDao issueStorage, IValidator<Issue> issueValidator)
        {
            _issueStorage = issueStorage;
            _issueValidator = issueValidator;
        }
        public Issue AddIssue(Issue issue, ref Response response)
        {
            if(IsNullIssueCheck(issue,ref response))
            {
                return issue;
            }
            var validationResult = _issueValidator.Validate(issue);
            if(!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return issue;
            }
            return _issueStorage.AddIssue(issue, ref response);

        }

        private bool IsNullIssueCheck(Issue issue, ref Response response)
        {
            if (issue == null)
            {
                ErrorsManager.AddFalseResponse(ref response, "Issue can not be null");
                return true;
            }
            return false;
        }

        public void DeleteIssue(int id, ref Response response)
        {
            _issueStorage.DeleteIssue(id, ref response);
        }

        public Issue GetIssueById(int id)
        {
            return _issueStorage.GetIssueById(id);
        }

        public Issue UpdateIssue(Issue updatedIssue, ref Response response)
        {
            if (IsNullIssueCheck(updatedIssue, ref response))
            {
                return updatedIssue;
            }
            var validationResult = _issueValidator.Validate(updatedIssue);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return updatedIssue;
            }
            return _issueStorage.AddIssue(updatedIssue, ref response);
        }

    }
}
