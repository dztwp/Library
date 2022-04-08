using System.Collections.Generic;

namespace Epam.Library.ErrorArchiver
{
    public class Response
    {
        public Response()
        {
            IsSuccess = true;
            ErrorCollection = new List<Error>();
        }
        public bool IsSuccess { get; set; }
        public ICollection<Error> ErrorCollection { get; set; }
    }
}
