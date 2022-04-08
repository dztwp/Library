using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace Epam.Library.MSSQLDAL
{
    public class PatentDao : IPatentDao
    {
        private SecureString _password;
        private static SqlCredential _loginAdmin;
        private static string _connectionString;

        public PatentDao()
        {
            _password = Helper.GetSecurityString("123");
            _loginAdmin = new SqlCredential("admin", _password);
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public Patent AddPatent(Patent patent, ref Response response)
        {
            if (IsPatentUnique(patent))
            {
                using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                {
                    SqlCommand command = _connection.CreateCommand();

                    command.CommandText = "Patent_Adding";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter IdParameter = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(IdParameter);

                    SqlParameter NameParameter = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = patent.Name,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NameParameter);

                    SqlParameter NoteParameter = new SqlParameter
                    {
                        ParameterName = "@note",
                        Value = patent.Note,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NoteParameter);

                    SqlParameter YearOfPublParameter = new SqlParameter
                    {
                        ParameterName = "@yearOfPubl",
                        Value = patent.YearOfPublishing,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(YearOfPublParameter);

                    SqlParameter CountryParameter = new SqlParameter
                    {
                        ParameterName = "@country",
                        Value = patent.Country,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(CountryParameter);

                    SqlParameter RegistrNumParameter = new SqlParameter
                    {
                        ParameterName = "@registrationNumber",
                        Value = patent.RegistrationNumber,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(RegistrNumParameter);

                    SqlParameter ApplicatDateParameter = new SqlParameter
                    {
                        ParameterName = "@applicationDate",
                        Value = patent.ApplicationDate,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(ApplicatDateParameter);

                    SqlParameter NumberOfPagesParameter = new SqlParameter
                    {
                        ParameterName = "@numberOfPages",
                        Value = patent.NumberOfPages,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NumberOfPagesParameter);

                    _connection.Open();
                    command.ExecuteNonQuery();

                    patent.Id = (int)IdParameter.Value;
                }
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "Patent with this registration number and country is already exist");
            }
            return patent;
        }

        private bool IsPatentUnique(Patent patent)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_IsPatentUnique";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter RegistrNumParameter = new SqlParameter
                {
                    ParameterName = "@registrationNumber",
                    Value = patent.RegistrationNumber,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(RegistrNumParameter);

                SqlParameter CountryParameter = new SqlParameter
                {
                    ParameterName = "@country",
                    Value = patent.Country,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(CountryParameter);

                _connection.Open();

                if ((int)command.ExecuteScalar() > 0)
                {
                    return false;
                }
            }
            return true;
        }
        public void AddPersonToPatent(int patentId, Person author)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_AddingAuthorToPatent";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter PatentIdParameter = new SqlParameter
                {
                    ParameterName = "@patentId",
                    Value = patentId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(PatentIdParameter);

                SqlParameter AuthorIdParameter = new SqlParameter
                {
                    ParameterName = "@authorId",
                    Value = author.Id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(AuthorIdParameter);

                _connection.Open();
                command.ExecuteNonQuery();
            }
        }

        internal static IEnumerable<Person> GetAuthorsOfPatent(int patentId)
        {
            var tmpListOfAuthors = new List<Person>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetPatentsAuthors";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter PatentIdParameter = new SqlParameter
                {
                    ParameterName = "@patentId",
                    Value = patentId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(PatentIdParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpPerson = new Person()
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"] as string,
                        LastName = reader["LastName"] as string
                    };
                    tmpListOfAuthors.Add(tmpPerson);
                }
            }
            return tmpListOfAuthors;
        }

        public void DeletePatent(int Id, ref Response response)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Obj_Deleting";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter PatentIdParameter = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = Id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(PatentIdParameter);

                _connection.Open();

                if (command.ExecuteNonQuery() == 0)
                {
                    ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
                }
            }
        }

        public IEnumerable<Patent> GetAllPatents()
        {
            List<Patent> tmpList = new List<Patent>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetAllPatents";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpPatent = new Patent()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        Country = reader["Country"] as string,
                        RegistrationNumber = reader["RegistrationNumber"] as string,
                        ApplicationDate = (int)reader["ApplicationDate"],
                        NumberOfPages = (int)reader["NumberOfPages"]
                    };
                    tmpPatent.Authors = GetAuthorsOfPatent(tmpPatent.Id);

                    tmpList.Add(tmpPatent);
                }
            }
            return tmpList;
        }

        public IEnumerable<Patent> GetOrderedByDescPatents()
        {
            List<Patent> tmpList = new List<Patent>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetOrderedByDescPatents";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpPatent = new Patent()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        Country = reader["Country"] as string,
                        RegistrationNumber = reader["RegistrationNumber"] as string,
                        ApplicationDate = (int)reader["ApplicationDate"],
                        NumberOfPages = (int)reader["NumberOfPages"]
                    };
                    tmpPatent.Authors = GetAuthorsOfPatent(tmpPatent.Id);

                    tmpList.Add(tmpPatent);
                }
            }
            return tmpList;
        }

        public IEnumerable<Patent> GetOrderedByPatents()
        {
            List<Patent> tmpList = new List<Patent>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetOrderedPatents";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpPatent = new Patent()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        Country = reader["Country"] as string,
                        RegistrationNumber = reader["RegistrationNumber"] as string,
                        ApplicationDate = (int)reader["ApplicationDate"],
                        NumberOfPages = (int)reader["NumberOfPages"]
                    };
                    tmpPatent.Authors = GetAuthorsOfPatent(tmpPatent.Id);

                    tmpList.Add(tmpPatent);
                }
            }
            return tmpList;
        }

        public IEnumerable<Patent> GetPatentByCharacterSet(string characterSet)
        {
            List<Patent> tmpList = new List<Patent>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetPatentsStartsWithsCharSet";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter CharSetParameter = new SqlParameter
                {
                    ParameterName = "@charSet",
                    Value = characterSet,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(CharSetParameter);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpPatent = new Patent()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        Country = reader["Country"] as string,
                        RegistrationNumber = reader["RegistrationNumber"] as string,
                        ApplicationDate = (int)reader["ApplicationDate"],
                        NumberOfPages = (int)reader["NumberOfPages"]
                    };
                    tmpPatent.Authors = GetAuthorsOfPatent(tmpPatent.Id);

                    tmpList.Add(tmpPatent);
                }
            }
            return tmpList;
        }

        public Patent GetPatentById(int id)
        {
            Patent tmpPatent = null;

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetPatentsStartsWithsCharSet";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter PatentIdParameter = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(PatentIdParameter);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tmpPatent = new Patent()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        Country = reader["Country"] as string,
                        RegistrationNumber = reader["RegistrationNumber"] as string,
                        ApplicationDate = (int)reader["ApplicationDate"],
                        NumberOfPages = (int)reader["NumberOfPages"]
                    };
                    tmpPatent.Authors = GetAuthorsOfPatent(tmpPatent.Id);
                }
            }
            return tmpPatent;
        }

        public IEnumerable<Patent> GetPatentsByAuthor(Person author)
        {
            List<Patent> tmpList = new List<Patent>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Patent_GetPatentsByAuthor";
                command.CommandType = CommandType.StoredProcedure;

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
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpPatent = new Patent()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        Country = reader["Country"] as string,
                        RegistrationNumber = reader["RegistrationNumber"] as string,
                        ApplicationDate = (int)reader["ApplicationDate"],
                        NumberOfPages = (int)reader["NumberOfPages"]
                    };
                    tmpPatent.Authors = GetAuthorsOfPatent(tmpPatent.Id);

                    tmpList.Add(tmpPatent);
                }
            }
            return tmpList;
        }

        public Patent UpdatePatent(Patent patent, ref Response response)
        {
            throw new NotImplementedException();
        }
    }
}
