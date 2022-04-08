using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using System.Security;
using System.Configuration;

namespace Epam.Library.MSSQLDAL
{
    public class BookDao : IBookDao
    {
        private SecureString _password;
        private static SqlCredential _loginAdmin;
        private static string _connectionString;

        public BookDao()
        {
            _password = Helper.GetSecurityString("123");
            _loginAdmin = new SqlCredential("admin", _password);
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public Book AddBook(Book book, ref Response response)
        {
            if (IsISBNExistAndUnique(book.ISBN) && IsBookUnique(book))
            {
                using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                {
                    SqlCommand command = _connection.CreateCommand();

                    command.CommandText = "Book_Adding";
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
                        Value = book.Name,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NameParameter);

                    SqlParameter NoteParameter = new SqlParameter
                    {
                        ParameterName = "@note",
                        Value = book.Note,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NoteParameter);

                    SqlParameter YearOfPublishingParameter = new SqlParameter
                    {
                        ParameterName = "@yearOfPubl",
                        Value = book.YearOfPublishing,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(YearOfPublishingParameter);

                    SqlParameter CityNameParameter = new SqlParameter
                    {
                        ParameterName = "@cityName",
                        Value = book.PlaceOfPublication,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(CityNameParameter);

                    SqlParameter PublishingHouseParameter = new SqlParameter
                    {
                        ParameterName = "@publishingHouse",
                        Value = book.PublishingHouse,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(PublishingHouseParameter);

                    SqlParameter NumberOfPagesParameter = new SqlParameter
                    {
                        ParameterName = "@numberOfPages",
                        Value = book.NumberOfPages,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(NumberOfPagesParameter);

                    SqlParameter ISBNParameter = new SqlParameter
                    {
                        ParameterName = "@isbn",
                        Value = book.ISBN,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(ISBNParameter);

                    _connection.Open();
                    command.ExecuteNonQuery();

                    book.Id = (int)IdParameter.Value;
                }
            }
            else
            {
                ErrorsManager.AddFalseResponse(ref response, "Book with this ISBN or name, authors and year of publishing is already exist");
            }
            return book;
        }

        private bool IsISBNExistAndUnique(string ISBN)
        {
            if (ISBN != null)
            {
                using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                {
                    SqlCommand command = _connection.CreateCommand();

                    command.CommandText = "Book_IsISSNExist";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter ISBNParameter = new SqlParameter
                    {
                        ParameterName = "@ISBN",
                        Value = ISBN,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(ISBNParameter);

                    _connection.Open();

                    if ((int)command.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsBookUnique(Book book)
        {
            bool isUnique = true;
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
                foreach (var author in book.Authors)
                {
                    using (_connection)
                    {
                        SqlCommand command = _connection.CreateCommand();

                        command.CommandText = "Book_IsBookAlreadyExist";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter NameParameter = new SqlParameter
                        {
                            ParameterName = "@name",
                            Value = book.Name,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(NameParameter);

                        SqlParameter YearOfPublishingParameter = new SqlParameter
                        {
                            ParameterName = "@yearOfPubl",
                            Value = book.YearOfPublishing,
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(YearOfPublishingParameter);

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
                        if ((int)command.ExecuteScalar() > 0)
                        {
                            isUnique = false;
                        }
                    }
                }
            return isUnique;
        }

        public void DeleteBook(int id, ref Response response)
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

        public IEnumerable<Book> GetAllBooks()
        {
            List<Book> tmpList = new List<Book>();
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetAllBooks";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpBook = new Book()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ISBN = reader["ISBN"] as string,
                    };
                    tmpBook.Authors = GetAuthorsOfBook(tmpBook.Id);
                    tmpList.Add(tmpBook);
                }
            }
            return tmpList;
        }

        internal static IEnumerable<Person> GetAuthorsOfBook(int bookId)
        {
            var tmpListOfAuthors = new List<Person>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetBooksAuthors";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter BookIdParameter = new SqlParameter
                {
                    ParameterName = "@bookId",
                    Value = bookId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(BookIdParameter);

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

        public Book GetBookById(int id)
        {
            Book tmpBook = null;

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetBookById";
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
                if (reader.Read())
                {
                    tmpBook = new Book()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ISBN = reader["ISBN"] as string
                    };
                    tmpBook.Authors = GetAuthorsOfBook(tmpBook.Id);
                }
            }
            return tmpBook;
        }

        public IEnumerable<Book> GetBooksByAuthor(Person author)
        {
            List<Book> tmpList = new List<Book>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetBookByAuthor";
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
                    var tmpBook = new Book()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ISBN = reader["ISBN"] as string
                    };
                    tmpBook.Authors = GetAuthorsOfBook(tmpBook.Id);

                    tmpList.Add(tmpBook);
                }
            }
            return tmpList;
        }

        public IEnumerable<Book> GetBooksByCharacterSet(string characterSet)
        {
            List<Book> tmpList = new List<Book>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetBooksStartWithCharacterSet";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter CharSetParameter = new SqlParameter
                {
                    ParameterName = "@inputString",
                    Value = characterSet,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(CharSetParameter);

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpBook = new Book()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ISBN = reader["ISBN"] as string
                    };
                    tmpBook.Authors = GetAuthorsOfBook(tmpBook.Id);

                    tmpList.Add(tmpBook);
                }
            }
            return tmpList;
        }

        public IEnumerable<Book> GetOrderedByBooks()
        {
            List<Book> tmpList = new List<Book>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetOrderedBooks";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpBook = new Book()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ISBN = reader["ISBN"] as string
                    };
                    tmpBook.Authors = GetAuthorsOfBook(tmpBook.Id);

                    tmpList.Add(tmpBook);
                }
            }
            return tmpList;
        }

        public IEnumerable<Book> GetOrderedByDescBooks()
        {
            List<Book> tmpList = new List<Book>();

            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_GetOrderedBooksByDesc";
                command.CommandType = CommandType.StoredProcedure;

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tmpBook = new Book()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] as string,
                        Note = reader["Note"] as string,
                        YearOfPublishing = (int)reader["YearOfPublishing"],
                        PlaceOfPublication = reader["CityName"] as string,
                        PublishingHouse = reader["PublishingHouse"] as string,
                        NumberOfPages = (int)reader["NumberOfPages"],
                        ISBN = reader["ISBN"] as string
                    };
                    tmpBook.Authors = GetAuthorsOfBook(tmpBook.Id);

                    tmpList.Add(tmpBook);
                }
            }
            return tmpList;
        }

        public Book UpdateBook(Book book, ref Response response)
        {
            throw new NotImplementedException();
        }

        public void AddPersonToBooks(int bookId, Person author)
        {
            using (var _connection = new SqlConnection(_connectionString, _loginAdmin))
            {
                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "Book_AddingAuthorToBook";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter BookIdParameter = new SqlParameter
                {
                    ParameterName = "@bookId",
                    Value = bookId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(BookIdParameter);

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
    }
}
