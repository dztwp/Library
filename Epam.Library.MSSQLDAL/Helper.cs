using System.Data.SqlClient;
using System.Security;

namespace Epam.Library.MSSQLDAL
{
    internal static class Helper
    {
        internal static  SecureString GetSecurityString (string pass)
        {
            var secString = new SecureString();
            for (int i = 0; i < pass.Length; i++)
            {
                secString.AppendChar(pass[i]);
            }
            secString.MakeReadOnly();
            return secString;
        }

    }
}
