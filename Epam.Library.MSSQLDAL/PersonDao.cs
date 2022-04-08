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
    public class PersonDao : IPersonDao
    {
        private SecureString _password;
        private static SqlCredential _loginAdmin;
        private static string _connectionString;

        public PersonDao()
        {
            _password = Helper.GetSecurityString("123");
            _loginAdmin = new SqlCredential("admin", _password);
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public Person AddPerson(Person author, ref Response response)
        {
            if (IsPersonNotExistInDB(author))
            {
                using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                {
                    SqlCommand command = _connection.CreateCommand();

                    command.CommandText = "Authors_Adding";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter IdParameter = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(IdParameter);

                    SqlParameter FirstNameParameter = new SqlParameter
                    {
                        ParameterName = "@firstName",
                        Value = author.FirstName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(FirstNameParameter);

                    SqlParameter LastNameParameter = new SqlParameter
                    {
                        ParameterName = "@lastName",
                        Value = author.LastName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(LastNameParameter);

                    _connection.Open();

                    author.Id = (int)IdParameter.Value;
                }
            }
            return author;
        }

        private bool IsPersonNotExistInDB(Person person)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Authors_IsPersonExistInDB";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter FirstNameParameter = new SqlParameter
                {
                    ParameterName = "@firstName",
                    Value = person.FirstName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(FirstNameParameter);

                SqlParameter LastNameParameter = new SqlParameter
                {
                    ParameterName = "@lastName",
                    Value = person.LastName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(LastNameParameter);

                _connection.Open();

                if ((int)command.ExecuteScalar() > 0)
                {
                    return false;
                }
            }
            return true;

        }
        public void DeletePerson(int id, ref Response response)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Obj_Deleting";
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
                    ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
                }
            }
        }

        public Person GetPersonById(int id)
        {
            Person tmpPerson = null;
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetPatentById";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter IdParameter = new SqlParameter
                {
                    ParameterName = "@patentId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(IdParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tmpPerson = new Person()
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    };
                }
            }
            return tmpPerson;
        }

        public Person UpdatePerson(Person author, ref Response response)
        {
            throw new NotImplementedException();
        }

        public int GetPersonIdByName(string firstName, string lastName)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Authors_GetIdByName";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter FirstNameParameter = new SqlParameter
                {
                    ParameterName = "@firstName",
                    Value = firstName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(FirstNameParameter);

                SqlParameter LastNameParameter = new SqlParameter
                {
                    ParameterName = "@lastName",
                    Value = lastName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(LastNameParameter);

                _connection.Open();

                return (int)command.ExecuteScalar();
            }
        }
    }
}
