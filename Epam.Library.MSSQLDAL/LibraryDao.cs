using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace Epam.Library.MSSQLDAL
{
    public class LibraryDao : ILibraryDao
    {
        private SecureString _password;
        private static SqlCredential _loginAdmin;
        private static string _connectionString;

        public LibraryDao()
        {
            _password = Helper.GetSecurityString("123");
            _loginAdmin = new SqlCredential("admin", _password);
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public IEnumerable<AbstractPaper> GetAll()
        {
            List<AbstractPaper> tmpList = new List<AbstractPaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Obj_GetAllPapers";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string typeOfObj = reader["TypeOfObj"] as string;
                    if (typeOfObj == "Book")
                    {
                        var tmpBook = new Book()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"] as string,
                            Note = reader["Note"] as string,
                            YearOfPublishing = (int)reader["YearOfPublishing"],
                            PlaceOfPublication = reader["BookCityName"] as string,
                            PublishingHouse = reader["BookPublishingHouse"] as string,
                            NumberOfPages = (int)reader["BookNumberOfPages"],
                            ISBN = reader["ISBN"] as string
                        };
                        tmpBook.Authors = BookDao.GetAuthorsOfBook(tmpBook.Id);

                        tmpList.Add(tmpBook);
                    }
                    if (typeOfObj == "Newspaper")
                    {
                        var tmpNewspaper = new Newspaper()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"] as string,
                            Note = reader["Note"] as string,
                            YearOfPublishing = (int)reader["YearOfPublishing"],
                            PlaceOfPublication = reader["NewspaperCityName"] as string,
                            PublishingHouse = reader["NewspaperPublishingHouse"] as string,
                            ISSN = reader["ISSN"] as string
                        };
                        tmpNewspaper.CollectionOfIssues = NewspaperDao.GetIssuesOfNewspaper(tmpNewspaper.Id);

                        tmpList.Add(tmpNewspaper);
                    }
                    if (typeOfObj == "Patent")
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
                            NumberOfPages = (int)reader["PatentNumberOfPages"]
                        };
                        tmpPatent.Authors = PatentDao.GetAuthorsOfPatent(tmpPatent.Id);

                        tmpList.Add(tmpPatent);
                    }
                }
            }
            return tmpList;
        }

        public IEnumerable<AbstractPaper> GetByName(string name)
        {
            List<AbstractPaper> tmpList = new List<AbstractPaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Obj_GetAllPapersByName";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NameParameter = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NameParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string typeOfObj = reader["TypeOfObj"] as string;
                    if (typeOfObj == "Book")
                    {
                        var tmpBook = new Book()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"] as string,
                            Note = reader["Note"] as string,
                            YearOfPublishing = (int)reader["YearOfPublishing"],
                            PlaceOfPublication = reader["BookCityName"] as string,
                            PublishingHouse = reader["BookPublishingHouse"] as string,
                            NumberOfPages = (int)reader["BookNumberOfPages"],
                            ISBN = reader["ISBN"] as string
                        };
                        tmpBook.Authors = BookDao.GetAuthorsOfBook(tmpBook.Id);

                        tmpList.Add(tmpBook);
                    }
                    if (typeOfObj == "Newspaper")
                    {
                        var tmpNewspaper = new Newspaper()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"] as string,
                            Note = reader["Note"] as string,
                            YearOfPublishing = (int)reader["YearOfPublishing"],
                            PlaceOfPublication = reader["NewspaperCityName"] as string,
                            PublishingHouse = reader["NewspaperPublishingHouse"] as string,
                            ISSN = reader["ISSN"] as string
                        };
                        tmpNewspaper.CollectionOfIssues = NewspaperDao.GetIssuesOfNewspaper(tmpNewspaper.Id);

                        tmpList.Add(tmpNewspaper);
                    }
                    if (typeOfObj == "Patent")
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
                            NumberOfPages = (int)reader["PatentNumberOfPages"]
                        };
                        tmpPatent.Authors = PatentDao.GetAuthorsOfPatent(tmpPatent.Id);

                        tmpList.Add(tmpPatent);
                    }
                }
            }
            return tmpList;
        }

        public IEnumerable<AbstractPaper> GroupByDate(int year)
        {
            List<AbstractPaper> tmpList = new List<AbstractPaper>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Obj_GetAllPapersGroupedByDate";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter NameParameter = new SqlParameter
                {
                    ParameterName = "@year",
                    Value = year,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(NameParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string typeOfObj = reader["TypeOfObj"] as string;
                    if (typeOfObj == "Book")
                    {
                        var tmpBook = new Book()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"] as string,
                            Note = reader["Note"] as string,
                            YearOfPublishing = (int)reader["YearOfPublishing"],
                            PlaceOfPublication = reader["BookCityName"] as string,
                            PublishingHouse = reader["BookPublishingHouse"] as string,
                            NumberOfPages = (int)reader["BookNumberOfPages"],
                            ISBN = reader["ISBN"] as string
                        };
                        tmpBook.Authors = BookDao.GetAuthorsOfBook(tmpBook.Id);

                        tmpList.Add(tmpBook);
                    }
                    if (typeOfObj == "Newspaper")
                    {
                        var tmpNewspaper = new Newspaper()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"] as string,
                            Note = reader["Note"] as string,
                            YearOfPublishing = (int)reader["YearOfPublishing"],
                            PlaceOfPublication = reader["NewspaperCityName"] as string,
                            PublishingHouse = reader["NewspaperPublishingHouse"] as string,
                            ISSN = reader["ISSN"] as string
                        };
                        tmpNewspaper.CollectionOfIssues = NewspaperDao.GetIssuesOfNewspaper(tmpNewspaper.Id);

                        tmpList.Add(tmpNewspaper);
                    }
                    if (typeOfObj == "Patent")
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
                            NumberOfPages = (int)reader["PatentNumberOfPages"]
                        };
                        tmpPatent.Authors = PatentDao.GetAuthorsOfPatent(tmpPatent.Id);

                        tmpList.Add(tmpPatent);
                    }
                }
            }
            return tmpList;
        }
    }
}
