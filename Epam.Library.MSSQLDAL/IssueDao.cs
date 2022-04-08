using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace Epam.Library.MSSQLDAL
{
    public class IssueDao : IIssueDao
    {
        private SecureString _password;
        private static SqlCredential _loginAdmin;
        private static string _connectionString;

        public IssueDao()
        {
            _password = Helper.GetSecurityString("123");
            _loginAdmin = new SqlCredential("admin", _password);
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public Issue AddIssue(Issue issue, ref Response response)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Issue_Adding";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter IdParameter = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdParameter);

                SqlParameter NumberOfIssueParameter = new SqlParameter
                {
                    ParameterName = "@numberOfIssue",
                    Value = issue.NumberOfIssue,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NumberOfIssueParameter);

                SqlParameter ReleaseDayParameter = new SqlParameter
                {
                    ParameterName = "@releaseDay",
                    Value = issue.ReleaseDay,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(ReleaseDayParameter);

                SqlParameter NumberOfPagesParameter = new SqlParameter
                {
                    ParameterName = "@numberOfPages",
                    Value = issue.NumberOfIssue,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NumberOfPagesParameter);

                _connection.Open();
                command.ExecuteNonQuery();

                issue.Id = (int)IdParameter.Value;
            }
            return issue;
        }

        public void DeleteIssue(int id, ref Response response)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Issue_Deleting";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter IdParameter = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(IdParameter);

                _connection.Open();
                if (command.ExecuteNonQuery() == 0)
                {
                    ErrorsManager.AddFalseResponse(ref response, "The specified newspaper does not exist in the data storage");
                }
            }
        }

        public Issue GetIssueById(int id)
        {
            Issue tmpIssue = null;
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Issue_GetIssueById";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter IdParameter = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(IdParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tmpIssue = new Issue()
                    {
                        Id = (int)reader["Id"],
                        NumberOfIssue = (int)reader["Name"],
                        ReleaseDay = (int)reader["Note"],
                        NumberOfPages = (int)reader["YearOfPublishing"]
                    };
                }
            }
            return tmpIssue;
        }

        public Issue UpdateIssue(Issue updatedIssue, ref Response response)
        {
            throw new NotImplementedException();
        }
    }
}
