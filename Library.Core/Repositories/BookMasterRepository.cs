using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Data;
using Library.Core.Models;
namespace Library.Core.Repositories
{
    public class BookRepository : BaseRepository, IBookMasterRepository
    {
        public List<BookMaster> GetAllBooks()
        {
            var books = new List<BookMaster>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT BookId, Title, Author, ISBN, TotalCopies, AvailableCopies, 
                            Category, CreatedDate, IsActive 
                            FROM BookMaster WHERE IsActive = 1 ORDER BY Title";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new BookMaster
                        {
                            BookId = Convert.ToInt32(reader["BookID"]),
                            Title = Convert.ToString(reader["Title"]),
                            Author = Convert.ToString(reader["Author"]),
                            ISBN = reader.IsDBNull(reader.GetOrdinal("ISBN")) ? null : Convert.ToString("isbn"),
                            TotalCopies = reader.GetInt32(reader.GetOrdinal("TotalCopies")),
                            AvailableCopies = reader.GetInt32(reader.GetOrdinal("AvailableCopies")),
                            Category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("createddate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("isactive"))
                        });
                    }
                }
            }

            return books;
        }
        public BookMaster GetBookById(int bookId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT BookId, Title, Author, ISBN, TotalCopies, AvailableCopies, 
                            Category, CreatedDate, IsActive 
                            FROM BookMaster WHERE BookId = @BookId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BookMaster
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Author = reader.GetString(reader.GetOrdinal("Author")),
                                ISBN = reader.IsDBNull( reader.GetOrdinal("ISBN")) ? null : reader.GetString(reader.GetOrdinal("ISBN")),
                                TotalCopies = reader.GetInt32(reader.GetOrdinal("TotalCopies")),
                                AvailableCopies = reader.GetInt32(reader.GetOrdinal("AvailableCopies")),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int AddBook(BookMaster book)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO BookMaster (Title, Author, ISBN, TotalCopies, AvailableCopies, Category, CreatedDate, IsActive)
                            OUTPUT INSERTED.BookId
                            VALUES (@Title, @Author, @ISBN, @TotalCopies, @AvailableCopies, @Category, GETDATE(), 1)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@ISBN", book.ISBN ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TotalCopies", book.TotalCopies);
                    command.Parameters.AddWithValue("@AvailableCopies", book.TotalCopies);
                    command.Parameters.AddWithValue("@Category", book.Category ?? "General");

                    return (int)command.ExecuteScalar();
                }
            }
        }

        public bool UpdateBook(BookMaster book)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE BookMaster SET 
                            Title = @Title, Author = @Author, ISBN = @ISBN, 
                            TotalCopies = @TotalCopies, Category = @Category
                            WHERE BookId = @BookId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", book.BookId);
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@ISBN", book.ISBN ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TotalCopies", book.TotalCopies);
                    command.Parameters.AddWithValue("@Category", book.Category ?? "General");

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBook(int bookId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = "UPDATE BookMaster SET IsActive = 0 WHERE BookId = @BookId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateAvailableCopies(int bookId, int copies)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = "UPDATE BookMaster SET AvailableCopies = @Copies WHERE BookId = @BookId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@Copies", copies);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<BookMaster> GetAvailableBooks()
        {
            var books = new List<BookMaster>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT BookId, Title, Author, ISBN, TotalCopies, AvailableCopies, 
                            Category, CreatedDate, IsActive 
                            FROM BookMaster 
                            WHERE IsActive = 1 AND AvailableCopies > 0 
                            ORDER BY Title";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new BookMaster
                        {
                            BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Author = reader.GetString(  reader.GetOrdinal("Author")),
                            ISBN = reader.IsDBNull(reader.GetOrdinal("ISBN")) ? null : reader.GetString(reader.GetOrdinal("ISBN")),
                            TotalCopies = reader.GetInt32(reader.GetOrdinal("TotalCopies")),
                            AvailableCopies = reader.GetInt32(reader.GetOrdinal("AvailableCopies")),
                            Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }

            return books;
        }
    }


        //public class BookMasterRepository : IBookMasterRepository
        //{
        //    private readonly string _connectionString;

        //    public BookMasterRepository()
        //    {
        //        _connectionString = ConfigurationManager.ConnectionStrings["Librarydb"].ConnectionString;
        //    }

        //    public List<BookMaster> GetAllBooks()
        //    {
        //        var list = new List<BookMaster>();
        //        using (var conn = new SqlConnection(_connectionString))
        //        using (var cmd = new SqlCommand("SELECT * FROM BookMaster", conn))
        //        {
        //            conn.Open();
        //            var reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                list.Add(new BookMaster
        //                {
        //                    BookId = Convert.ToInt32(reader["BookId"]),
        //                    Title = reader["Title"].ToString(),
        //                    Author = reader["Author"].ToString(),
        //                    ISBN = reader["ISBN"].ToString(),
        //                    TotalCopies = Convert.ToInt32(reader["TotalCopies"]),
        //                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"])
        //                });
        //            }
        //        }
        //        return list;
        //    }

        //    public BookMaster GetBookById(int id)
        //    {
        //        BookMaster book = null;
        //        using (var conn = new SqlConnection(_connectionString))
        //        using (var cmd = new SqlCommand("SELECT * FROM BookMaster WHERE BookId = @BookId", conn))
        //        {
        //            cmd.Parameters.AddWithValue("@BookId", id);
        //            conn.Open();
        //            var reader = cmd.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                book = new BookMaster
        //                {
        //                    BookId = Convert.ToInt32(reader["BookId"]),
        //                    Title = reader["Title"].ToString(),
        //                    Author = reader["Author"].ToString(),
        //                    ISBN = reader["ISBN"].ToString(),
        //                    TotalCopies = Convert.ToInt32(reader["TotalCopies"]),
        //                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"])
        //                };
        //            }
        //        }
        //        return book;
        //    }

        //    public void AddBook(BookMaster book)
        //    {
        //        using (var conn = new SqlConnection(_connectionString))
        //        using (var cmd = new SqlCommand(@"INSERT INTO BookMaster (Title, Author, ISBN, TotalCopies, AvailableCopies) 
        //                                          VALUES (@Title, @Author, @ISBN, @TotalCopies, @AvailableCopies)", conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Title", book.Title);
        //            cmd.Parameters.AddWithValue("@Author", book.Author);
        //            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
        //            cmd.Parameters.AddWithValue("@TotalCopies", book.TotalCopies);
        //            cmd.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //    public void UpdateBook(BookMaster book)
        //    {
        //        using (var conn = new SqlConnection(_connectionString))
        //        using (var cmd = new SqlCommand(@"UPDATE BookMaster SET Title = @Title, Author = @Author, ISBN = @ISBN, 
        //                                          TotalCopies = @TotalCopies, AvailableCopies = @AvailableCopies 
        //                                          WHERE BookId = @BookId", conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Title", book.Title);
        //            cmd.Parameters.AddWithValue("@Author", book.Author);
        //            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
        //            cmd.Parameters.AddWithValue("@TotalCopies", book.TotalCopies);
        //            cmd.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);
        //            cmd.Parameters.AddWithValue("@BookId", book.BookId);

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //    public void DeleteBook(int id)
        //    {
        //        using (var conn = new SqlConnection(_connectionString))
        //        using (var cmd = new SqlCommand("DELETE FROM BookMaster WHERE BookId = @BookId", conn))
        //        {
        //            cmd.Parameters.AddWithValue("@BookId", id);
        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }


        //    public void UpdateAvailableCopies(int bookId, int newAvailableCopies)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
}
