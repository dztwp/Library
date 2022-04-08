using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;

namespace Epam.Library.BLL.Interfaces
{
    public interface IIssueLogic
    {
        Issue AddIssue(Issue issue, ref Response response);

        void DeleteIssue(int id, ref Response response);

        Issue UpdateIssue(Issue updatedIssue, ref Response response);

        Issue GetIssueById(int id);
    }
}
