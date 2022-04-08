using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System.Collections.Generic;
using System.Linq;

namespace Epam.Library.DAL.FakeDAL
{
    public class IssueDao : IIssueDao
    {
        public Issue AddIssue(Issue issue, ref Response response)
        {
            issue.Id = GetNewId();
            DataStorage.IssueStorage.Add(issue.Id, issue);
            return issue;
        }
        private int GetNewId()
        {
            return DataStorage.IssueStorage.Count + 1;
        }

        public void DeleteIssue(int id, ref Response response)
        {
            if (DataStorage.IssueStorage.Remove(id))
            {
                DeleteIssueInCollection(DataStorage.Storage.Values.OfType<Newspaper>(), id);
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "The specified newspaper does not exist in the data storage");
            }
        }

        private void DeleteIssueInCollection(IEnumerable<Newspaper> newspapers, int id)
        {
            foreach (var newspaper in newspapers)
            {
                newspaper.CollectionOfIssues.ToList().RemoveAll(x => x.Id == id);
            }
        }
        public Issue GetIssueById(int id)
        {
            return DataStorage.IssueStorage.Values
                .FirstOrDefault(issue => issue.Id == id);
        }

        public Issue UpdateIssue(Issue updatedIssue, ref Response response)
        {
            throw new System.NotImplementedException();
        }
    }
}
