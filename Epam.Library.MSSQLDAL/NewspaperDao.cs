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
    public class NewspaperDao : INewspaperDao
    {
        private SecureString _password;
        private static SqlCredential _loginAdmin;
        private static string _connectionString;

        public NewspaperDao()
        {
            _password = Helper.GetSecurityString("123");
            _loginAdmin = new SqlCredential("admin", _password);
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public Newspaper AddIssueToNewspaper(Issue newIssue, Newspaper newspaper, ref Response response)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_AddingIssueToNewspaper";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NewspaperIdParameter = new SqlParameter
                {
                    ParameterName = "@newspaperId",
                    SqlDbType = SqlDbType.Int,
                    Value = newspaper.Id,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NewspaperIdParameter);

                SqlParameter IssueIdParameter = new SqlParameter
                {
                    ParameterName = "@issueId",
                    SqlDbType = SqlDbType.Int,
                    Value = newIssue.Id,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(IssueIdParameter);

                _connection.Open();

                command.ExecuteNonQuery();
            }
            newspaper.CollectionOfIssues = new List<Issue> { newIssue };
            return newspaper;
        }

        public Newspaper AddNewspaper(Newspaper newspaper, ref Response response)
        {
            if (IsISSNCorrect(newspaper))
            {
                return NewspaperAddingDBase(newspaper, ref response);
            }
            ErrorsManager.AddFalseResponse(ref response, "Newspaper with this ISSN already exist, but the names don't match");
            return newspaper;
        }

        private Newspaper NewspaperAddingDBase(Newspaper newspaper, ref Response response)
        {
            if (IsNewspaperUnique(newspaper))
            {
                using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                {
                    SqlCommand command = _connection.CreateCommand();

                    command.CommandText = "Newspaper_Adding";
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
                        Value = newspaper.Name,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NameParameter);

                    SqlParameter NoteParameter = new SqlParameter
                    {
                        ParameterName = "@note",
                        Value = newspaper.Note,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NoteParameter);

                    SqlParameter YearOfPublishParameter = new SqlParameter
                    {
                        ParameterName = "@yearOfPubl",
                        Value = newspaper.YearOfPublishing,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(YearOfPublishParameter);

                    SqlParameter CityNameParameter = new SqlParameter
                    {
                        ParameterName = "@cityName",
                        Value = newspaper.PlaceOfPublication,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(CityNameParameter);

                    SqlParameter PublishingHouseParameter = new SqlParameter
                    {
                        ParameterName = "@publishingHouse",
                        Value = newspaper.PublishingHouse,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(PublishingHouseParameter);

                    SqlParameter ISSNParameter = new SqlParameter
                    {
                        ParameterName = "@ISSN",
                        Value = newspaper.ISSN,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(ISSNParameter);

                    _connection.Open();
                    command.ExecuteNonQuery();

                    newspaper.Id = (int)IdParameter.Value;
                }
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "Newspaper with this name, publishing house and year of publishing is already exist");
            }
            return newspaper;
        }

        private bool IsNewspaperUnique(Newspaper newspaper)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_IsNewspaperUnique";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NameParameter = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = newspaper.Name,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NameParameter);

                SqlParameter PublishingHouseParameter = new SqlParameter
                {
                    ParameterName = "@publishingHouse",
                    Value = newspaper.PublishingHouse,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(PublishingHouseParameter);

                SqlParameter YearOfPublishParameter = new SqlParameter
                {
                    ParameterName = "@yearOfPublishing",
                    Value = newspaper.YearOfPublishing,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(YearOfPublishParameter);

                _connection.Open();

                if ((int)command.ExecuteScalar() > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsISSNCorrect(Newspaper newspaper)
        {
            if (IsISSNExistInStorage(newspaper.ISSN))
            {
                using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                {
                    SqlCommand command = _connection.CreateCommand();

                    command.CommandText = "Newspaper_IsISSNAndNameEqualsDataInDB";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter NameParameter = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = newspaper.Name,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NameParameter);

                    SqlParameter ISSNParameter = new SqlParameter
                    {
                        ParameterName = "@issn",
                        Value = newspaper.ISSN,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(ISSNParameter);

                    _connection.Open();

                    if ((int)command.ExecuteScalar() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        private static bool IsISSNExistInStorage(string issn)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_IsISSNExistInStorage";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter ISSNParameter = new SqlParameter
                {
                    ParameterName = "@issn",
                    Value = issn,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(ISSNParameter);

                _connection.Open();
                if ((int)command.ExecuteScalar() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void DeleteNewspaper(int id, ref Response response)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Obj_Deleting";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter ISSNParameter = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(ISSNParameter);

                _connection.Open();

                if (command.ExecuteNonQuery() == 0)
                {
                    ErrorsManager.AddFalseResponse(ref response, "There is no data with the specified id in the data storage");
                }
            }
        }

        public IEnumerable<Newspaper> GetAllNewspapers()
        {
            List<Newspaper> tmpList = new List<Newspaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_GetAllNewspapers";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpNewspaper = new Newspaper()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        ISSN = reader["ISSN"] as string
                    };
                    tmpNewspaper.CollectionOfIssues = GetIssuesOfNewspaper(tmpNewspaper.Id);

                    tmpList.Add(tmpNewspaper);
                }
            }
            return tmpList;
        }

        internal static IEnumerable<Issue> GetIssuesOfNewspaper(int newspaperId)
        {
            var tmpListOfIssues = new List<Issue>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_GetIssuesOfNewspaper";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NewspaperIdParameter = new SqlParameter
                {
                    ParameterName = "@newspaperId",
                    Value = newspaperId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NewspaperIdParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpIssue = new Issue()
                    {
                        Id = (int)reader["Id"],
                        NumberOfIssue = (int)reader["NumberOfIssue"],
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ReleaseDay = (int)reader["ReleaseDay"]
                    };
                    
                    tmpListOfIssues.Add(tmpIssue);
                }
            }
            return tmpListOfIssues;
        }

        public Newspaper GetNewspaperById(int id)
        {
            Newspaper tmpNewspaper = null;

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();
                command.CommandText = "Newspaper_GetAllNewspapers";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NewspaperIdParameter = new SqlParameter
                {
                    ParameterName = "@newspaperId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NewspaperIdParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tmpNewspaper = new Newspaper()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        ISSN = reader["ISSN"] as string
                    };
                    tmpNewspaper.CollectionOfIssues = GetIssuesOfNewspaper(tmpNewspaper.Id);
                }
            }
            return tmpNewspaper;
        }

        public IEnumerable<Newspaper> GetNewspapersByCharacterSet(string characterSet)
        {
            List<Newspaper> tmpList = new List<Newspaper>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_GetNewspapersStartsWithsCharSet";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NewspaperIdParameter = new SqlParameter
                {
                    ParameterName = "@charSet",
                    Value = characterSet,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NewspaperIdParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpNewspaper = new Newspaper()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        ISSN = reader["ISSN"] as string
                    };
                    tmpNewspaper.CollectionOfIssues = GetIssuesOfNewspaper(tmpNewspaper.Id);

                    tmpList.Add(tmpNewspaper);
                }
            }
            return tmpList;
        }

        public IEnumerable<Newspaper> GetNewspapersByPublishingHouse(string publishingHouse)
        {
            List<Newspaper> tmpList = new List<Newspaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_GetNewspapersByPublishingHouse";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@publishingHouse", publishingHouse);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpNewspaper = new Newspaper()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        ISSN = reader["ISSN"] as string
                    };
                    tmpNewspaper.CollectionOfIssues = GetIssuesOfNewspaper(tmpNewspaper.Id);

                    tmpList.Add(tmpNewspaper);
                }
            }
            return tmpList;
        }

        public IEnumerable<Newspaper> GetOrderedByDescNewspapers()
        {
            List<Newspaper> tmpList = new List<Newspaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_GetOrderedByDescNewspapers";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpNewspaper = new Newspaper()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        ISSN = reader["ISSN"] as string
                    };
                    tmpNewspaper.CollectionOfIssues = GetIssuesOfNewspaper(tmpNewspaper.Id);

                    tmpList.Add(tmpNewspaper);
                }
            }
            return tmpList;
        }

        public IEnumerable<Newspaper> GetOrderedByNewspapers()
        {
            List<Newspaper> tmpList = new List<Newspaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Newspaper_GetOrderedNewspapers";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpNewspaper = new Newspaper()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        ISSN = reader["ISSN"] as string
                    };
                    tmpNewspaper.CollectionOfIssues = GetIssuesOfNewspaper(tmpNewspaper.Id);

                    tmpList.Add(tmpNewspaper);
                }
            }
            return tmpList;
        }

        public Newspaper UpdateNewspaper(Newspaper newspaper, ref Response response)
        {
            throw new NotImplementedException();
        }
    }
}
