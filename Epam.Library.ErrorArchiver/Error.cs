namespace Epam.Library.ErrorArchiver
{
    public class Error
    {
        public string ErrorDescription { get; set; }
        public Error(string errorText)
        {
            ErrorDescription = errorText;
        }
    }
}
