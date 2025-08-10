using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Data;
using Library.Core.Models;
using Library.Core.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace Library.Core.Repositories
{
    public class BookIssueRepository : BaseRepository, IBookIssueRepository
    {
        public List<BookIssue> GetAllIssues()
        {
            var issues = new List<BookIssue>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
                           bi.ReturnDate, bi.FineAmount, bi.IsReturned,
                           bm.Title as BookTitle, bm.Author as BookAuthor,
                           mm.FullName as MemberName, mm.Mobile as MemberMobile
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
                    ORDER BY bi.IssueDate DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        issues.Add(MapBookIssue(reader));
                    }
                }
            }

            return issues;
        }

        public List<BookIssue> GetActiveIssues()
        {
            var issues = new List<BookIssue>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
                           bi.ReturnDate, bi.FineAmount, bi.IsReturned,
                           bm.Title as BookTitle, bm.Author as BookAuthor,
                           mm.FullName as MemberName, mm.Mobile as MemberMobile
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
                    WHERE bi.IsReturned = 0
                    ORDER BY bi.DueDate ASC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        issues.Add(MapBookIssue(reader));
                    }
                }
            }

            return issues;
        }

        public List<BookIssue> GetOverdueIssues()
        {
            var issues = new List<BookIssue>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
                           bi.ReturnDate, bi.FineAmount, bi.IsReturned,
                           bm.Title as BookTitle, bm.Author as BookAuthor,
                           mm.FullName as MemberName, mm.Mobile as MemberMobile
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
                    WHERE bi.IsReturned = 0 AND bi.DueDate < GETDATE()
                    ORDER BY bi.DueDate ASC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        issues.Add(MapBookIssue(reader));
                    }
                }
            }

            return issues;
        }

        public BookIssue GetIssueById(int issueId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
                           bi.ReturnDate, bi.FineAmount, bi.IsReturned,
                           bm.Title as BookTitle, bm.Author as BookAuthor,
                           mm.FullName as MemberName, mm.Mobile as MemberMobile
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
                    WHERE bi.IssueId = @IssueId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IssueId", issueId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapBookIssue(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<BookIssue> GetBookHistory(int bookId)
        {
            var issues = new List<BookIssue>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
                           bi.ReturnDate, bi.FineAmount, bi.IsReturned,
                           bm.Title as BookTitle, bm.Author as BookAuthor,
                           mm.FullName as MemberName, mm.Mobile as MemberMobile
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
                    WHERE bi.BookId = @BookId
                    ORDER BY bi.IssueDate DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            issues.Add(MapBookIssue(reader));
                        }
                    }
                }
            }

            return issues;
        }

        public List<BookIssue> GetMemberHistory(int memberId)
        {
            var issues = new List<BookIssue>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
                           bi.ReturnDate, bi.FineAmount, bi.IsReturned,
                           bm.Title as BookTitle, bm.Author as BookAuthor,
                           mm.FullName as MemberName, mm.Mobile as MemberMobile
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
                    WHERE bi.MemberId = @MemberId
                    ORDER BY bi.IssueDate DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", memberId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            issues.Add(MapBookIssue(reader));
                        }
                    }
                }
            }

            return issues;
        }

        public int IssueBook(BookIssue bookIssue)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert book issue record
                        var insertQuery = @"
                            INSERT INTO BookIssue (BookId, MemberId, IssueDate, DueDate, IsReturned)
                            OUTPUT INSERTED.IssueId
                            VALUES (@BookId, @MemberId, @IssueDate, @DueDate, 0)";

                        int issueId;
                        using (var command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookId", bookIssue.BookId);
                            command.Parameters.AddWithValue("@MemberId", bookIssue.MemberId);
                            command.Parameters.AddWithValue("@IssueDate", bookIssue.IssueDate);
                            command.Parameters.AddWithValue("@DueDate", bookIssue.DueDate);

                            issueId = (int)command.ExecuteScalar();
                        }

                        // Update available copies
                        var updateQuery = @"
                            UPDATE BookMaster 
                            SET AvailableCopies = AvailableCopies - 1 
                            WHERE BookId = @BookId AND AvailableCopies > 0";

                        using (var command = new SqlCommand(updateQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookId", bookIssue.BookId);

                            if (command.ExecuteNonQuery() == 0)
                            {
                                throw new InvalidOperationException("Book is not available for issue");
                            }
                        }

                        transaction.Commit();
                        return issueId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool ReturnBook(int issueId, decimal? fineAmount)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Get the book issue details
                        var getBookQuery = "SELECT BookId FROM BookIssue WHERE IssueId = @IssueId";
                        int bookId;

                        using (var command = new SqlCommand(getBookQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IssueId", issueId);
                            bookId = (int)command.ExecuteScalar();
                        }

                        // Update book issue record
                        var updateIssueQuery = @"
                            UPDATE BookIssue 
                            SET ReturnDate = GETDATE(), FineAmount = @FineAmount, IsReturned = 1
                            WHERE IssueId = @IssueId";

                        using (var command = new SqlCommand(updateIssueQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IssueId", issueId);
                            command.Parameters.AddWithValue("@FineAmount", fineAmount ?? (object)DBNull.Value);
                            command.ExecuteNonQuery();
                        }

                        // Update available copies
                        var updateBookQuery = @"
                            UPDATE BookMaster 
                            SET AvailableCopies = AvailableCopies + 1 
                            WHERE BookId = @BookId";

                        using (var command = new SqlCommand(updateBookQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookId", bookId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<ChartData> GetIssuesPerDay(int days)
        {
            var chartData = new List<ChartData>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT CONVERT(DATE, IssueDate) as IssueDate, COUNT(*) as IssueCount
                    FROM BookIssue
                    WHERE IssueDate >= DATEADD(DAY, -@Days, GETDATE())
                    GROUP BY CONVERT(DATE, IssueDate)
                    ORDER BY IssueDate";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Days", days);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chartData.Add(new ChartData
                            {
                                Label = reader.GetDateTime(reader.GetOrdinal("IssueDate")).ToString("MM/dd"),
                                Value = reader.GetInt32(reader.GetOrdinal("IssueCount"))
                            });
                        }
                    }
                }
            }

            return chartData;
        }

        public List<ChartData> GetBooksByCategory()
        {
            var chartData = new List<ChartData>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT bm.Category, COUNT(bi.IssueId) as IssueCount
                    FROM BookIssue bi
                    INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
                    GROUP BY bm.Category
                    ORDER BY IssueCount DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chartData.Add(new ChartData
                        {
                            Label = reader.GetString(reader.GetOrdinal("Category"))?? "General",
                            Value = reader.GetInt32(reader.GetOrdinal("IssueCount"))
                        });
                    }
                }
            }

            return chartData;
        }

        public DashboardViewModel GetDashboardData()
        {
            var dashboard = new DashboardViewModel();

            using (var connection = GetConnection())
            {
                connection.Open();

                // Get summary counts
                var summaryQuery = @"
                    SELECT 
                        (SELECT COUNT(*) FROM BookMaster WHERE IsActive = 1) as TotalBooks,
                        (SELECT COUNT(*) FROM MemberMaster WHERE IsActive = 1) as TotalMembers,
                        (SELECT COUNT(*) FROM BookIssue WHERE IsReturned = 0) as ActiveIssues,
                        (SELECT COUNT(*) FROM BookIssue WHERE IsReturned = 0 AND DueDate < GETDATE()) as OverdueBooks";

                using (var command = new SqlCommand(summaryQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dashboard.TotalBooks = reader.GetInt32(reader.GetOrdinal("TotalBooks"));
                        dashboard.TotalMembers = reader.GetInt32(reader.GetOrdinal("TotalMembers"));
                        dashboard.ActiveIssues = reader.GetInt32(reader.GetOrdinal("ActiveIssues"));
                        dashboard.OverdueBooks = reader.GetInt32(reader.GetOrdinal("OverdueBooks"));
                    }
                }
            }

            dashboard.IssuesPerDay = GetIssuesPerDay(7);
            dashboard.BooksByCategory = GetBooksByCategory();

            return dashboard;
        }

        private BookIssue MapBookIssue(SqlDataReader reader)
        {
            return new BookIssue
            {
                IssueId = reader.GetInt32(reader.GetOrdinal("IssueId")),
                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                MemberId = reader.GetInt32(reader.GetOrdinal("MemberId")),
                IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                FineAmount = reader.IsDBNull(reader.GetOrdinal("FineAmount"))? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("FineAmount")),
                IsReturned = reader.GetBoolean(reader.GetOrdinal("IsReturned")),
                BookTitle = reader.GetString(reader.GetOrdinal("BookTitle")),
                BookAuthor = reader.GetString(reader.GetOrdinal("BookAuthor")),
                MemberName = reader.GetString(reader.GetOrdinal("MemberName")),
                MemberMobile = reader.GetString(reader.GetOrdinal("MemberMobile"))
            };
        }
       
        //public List<BookIssue> GetAllIssues()
        //{
        //    var issues = new List<BookIssue>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
        //                   bi.ReturnDate, bi.FineAmount, bi.IsReturned,
        //                   bm.Title as BookTitle, bm.Author as BookAuthor,
        //                   mm.FullName as MemberName, mm.Mobile as MemberMobile
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
        //            ORDER BY bi.IssueDate DESC";

        //        using (var command = new SqlCommand(query, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                issues.Add(MapBookIssue(reader));
        //            }
        //        }
        //    }

        //    return issues;
        //}

        //public List<BookIssue> GetActiveIssues()
        //{
        //    var issues = new List<BookIssue>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
        //                   bi.ReturnDate, bi.FineAmount, bi.IsReturned,
        //                   bm.Title as BookTitle, bm.Author as BookAuthor,
        //                   mm.FullName as MemberName, mm.Mobile as MemberMobile
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
        //            WHERE bi.IsReturned = 0
        //            ORDER BY bi.DueDate ASC";

        //        using (var command = new SqlCommand(query, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                issues.Add(MapBookIssue(reader));
        //            }
        //        }
        //    }

        //    return issues;
        //}

        //public List<BookIssue> GetOverdueIssues()
        //{
        //    var issues = new List<BookIssue>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
        //                   bi.ReturnDate, bi.FineAmount, bi.IsReturned,
        //                   bm.Title as BookTitle, bm.Author as BookAuthor,
        //                   mm.FullName as MemberName, mm.Mobile as MemberMobile
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
        //            WHERE bi.IsReturned = 0 AND bi.DueDate < GETDATE()
        //            ORDER BY bi.DueDate ASC";

        //        using (var command = new SqlCommand(query, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                issues.Add(MapBookIssue(reader));
        //            }
        //        }
        //    }

        //    return issues;
        //}

        //public BookIssue GetIssueById(int issueId)
        //{
        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
        //                   bi.ReturnDate, bi.FineAmount, bi.IsReturned,
        //                   bm.Title as BookTitle, bm.Author as BookAuthor,
        //                   mm.FullName as MemberName, mm.Mobile as MemberMobile
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
        //            WHERE bi.IssueId = @IssueId";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@IssueId", issueId);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    return MapBookIssue(reader);
        //                }
        //            }
        //        }
        //    }

        //    return null;
        //}

        //public List<BookIssue> GetBookHistory(int bookId)
        //{
        //    var issues = new List<BookIssue>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
        //                   bi.ReturnDate, bi.FineAmount, bi.IsReturned,
        //                   bm.Title as BookTitle, bm.Author as BookAuthor,
        //                   mm.FullName as MemberName, mm.Mobile as MemberMobile
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
        //            WHERE bi.BookId = @BookId
        //            ORDER BY bi.IssueDate DESC";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@BookId", bookId);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    issues.Add(MapBookIssue(reader));
        //                }
        //            }
        //        }
        //    }

        //    return issues;
        //}

        //public List<BookIssue> GetMemberHistory(int memberId)
        //{
        //    var issues = new List<BookIssue>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bi.IssueId, bi.BookId, bi.MemberId, bi.IssueDate, bi.DueDate, 
        //                   bi.ReturnDate, bi.FineAmount, bi.IsReturned,
        //                   bm.Title as BookTitle, bm.Author as BookAuthor,
        //                   mm.FullName as MemberName, mm.Mobile as MemberMobile
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            INNER JOIN MemberMaster mm ON bi.MemberId = mm.MemberId
        //            WHERE bi.MemberId = @MemberId
        //            ORDER BY bi.IssueDate DESC";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@MemberId", memberId);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    issues.Add(MapBookIssue(reader));
        //                }
        //            }
        //        }
        //    }

        //    return issues;
        //}

        //public int IssueBook(BookIssue bookIssue)
        //{
        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Insert book issue record
        //                var insertQuery = @"
        //                    INSERT INTO BookIssue (BookId, MemberId, IssueDate, DueDate, IsReturned)
        //                    OUTPUT INSERTED.IssueId
        //                    VALUES (@BookId, @MemberId, @IssueDate, @DueDate, 0)";

        //                int issueId;
        //                using (var command = new SqlCommand(insertQuery, connection, transaction))
        //                {
        //                    command.Parameters.AddWithValue("@BookId", bookIssue.BookId);
        //                    command.Parameters.AddWithValue("@MemberId", bookIssue.MemberId);
        //                    command.Parameters.AddWithValue("@IssueDate", bookIssue.IssueDate);
        //                    command.Parameters.AddWithValue("@DueDate", bookIssue.DueDate);

        //                    issueId = (int)command.ExecuteScalar();
        //                }

        //                // Update available copies
        //                var updateQuery = @"
        //                    UPDATE BookMaster 
        //                    SET AvailableCopies = AvailableCopies - 1 
        //                    WHERE BookId = @BookId AND AvailableCopies > 0";

        //                using (var command = new SqlCommand(updateQuery, connection, transaction))
        //                {
        //                    command.Parameters.AddWithValue("@BookId", bookIssue.BookId);

        //                    if (command.ExecuteNonQuery() == 0)
        //                    {
        //                        throw new InvalidOperationException("Book is not available for issue");
        //                    }
        //                }

        //                transaction.Commit();
        //                return issueId;
        //            }
        //            catch
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}

        //public bool ReturnBook(int issueId, decimal? fineAmount)
        //{
        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Get the book issue details
        //                var getBookQuery = "SELECT BookId FROM BookIssue WHERE IssueId = @IssueId";
        //                int bookId;

        //                using (var command = new SqlCommand(getBookQuery, connection, transaction))
        //                {
        //                    command.Parameters.AddWithValue("@IssueId", issueId);
        //                    bookId = (int)command.ExecuteScalar();
        //                }

        //                // Update book issue record
        //                var updateIssueQuery = @"
        //                    UPDATE BookIssue 
        //                    SET ReturnDate = GETDATE(), FineAmount = @FineAmount, IsReturned = 1
        //                    WHERE IssueId = @IssueId";

        //                using (var command = new SqlCommand(updateIssueQuery, connection, transaction))
        //                {
        //                    command.Parameters.AddWithValue("@IssueId", issueId);
        //                    command.Parameters.AddWithValue("@FineAmount", fineAmount ?? (object)DBNull.Value);
        //                    command.ExecuteNonQuery();
        //                }

        //                // Update available copies
        //                var updateBookQuery = @"
        //                    UPDATE BookMaster 
        //                    SET AvailableCopies = AvailableCopies + 1 
        //                    WHERE BookId = @BookId";

        //                using (var command = new SqlCommand(updateBookQuery, connection, transaction))
        //                {
        //                    command.Parameters.AddWithValue("@BookId", bookId);
        //                    command.ExecuteNonQuery();
        //                }

        //                transaction.Commit();
        //                return true;
        //            }
        //            catch
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}

        //public List<ChartData> GetIssuesPerDay(int days)
        //{
        //    var chartData = new List<ChartData>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT CONVERT(DATE, IssueDate) as IssueDate, COUNT(*) as IssueCount
        //            FROM BookIssue
        //            WHERE IssueDate >= DATEADD(DAY, -@Days, GETDATE())
        //            GROUP BY CONVERT(DATE, IssueDate)
        //            ORDER BY IssueDate";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Days", days);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    chartData.Add(new ChartData
        //                    {
        //                        Label = reader.GetDateTime("IssueDate").ToString("MM/dd"),
        //                        Value = reader.GetInt32("IssueCount")
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return chartData;
        //}

        //public List<ChartData> GetBooksByCategory()
        //{
        //    var chartData = new List<ChartData>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        var query = @"
        //            SELECT bm.Category, COUNT(bi.IssueId) as IssueCount
        //            FROM BookIssue bi
        //            INNER JOIN BookMaster bm ON bi.BookId = bm.BookId
        //            GROUP BY bm.Category
        //            ORDER BY IssueCount DESC";

        //        using (var command = new SqlCommand(query, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                chartData.Add(new ChartData
        //                {
        //                    Label = reader.GetString("Category") ?? "General",
        //                    Value = reader.GetInt32("IssueCount")
        //                });
        //            }
        //        }
        //    }

        //    return chartData;
        //}

        //public DashboardViewModel GetDashboardData()
        //{
        //    var dashboard = new DashboardViewModel();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();

        //        // Get summary counts
        //        var summaryQuery = @"
        //            SELECT 
        //                (SELECT COUNT(*) FROM BookMaster WHERE IsActive = 1) as TotalBooks,
        //                (SELECT COUNT(*) FROM MemberMaster WHERE IsActive = 1) as TotalMembers,
        //                (SELECT COUNT(*) FROM BookIssue WHERE IsReturned = 0) as ActiveIssues,
        //                (SELECT COUNT(*) FROM BookIssue WHERE IsReturned = 0 AND DueDate < GETDATE()) as OverdueBooks";

        //        using (var command = new SqlCommand(summaryQuery, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                dashboard.TotalBooks = reader.GetInt32("TotalBooks");
        //                dashboard.TotalMembers = reader.GetInt32("TotalMembers");
        //                dashboard.ActiveIssues = reader.GetInt32("ActiveIssues");
        //                dashboard.OverdueBooks = reader.GetInt32("OverdueBooks");
        //            }
        //        }
        //    }

        //    dashboard.IssuesPerDay = GetIssuesPerDay(7);
        //    dashboard.BooksByCategory = GetBooksByCategory();

        //    return dashboard;
        //}

        //private BookIssue MapBookIssue(SqlDataReader reader)
        //{
        //    return new BookIssue
        //    {
        //        IssueId = reader.GetInt32("IssueId"),
        //        BookId = reader.GetInt32("BookId"),
        //        MemberId = reader.GetInt32("MemberId"),
        //        IssueDate = reader.GetDateTime("IssueDate"),
        //        DueDate = reader.GetDateTime("DueDate"),
        //        ReturnDate = reader.IsDBNull("ReturnDate") ? (DateTime?)null : reader.GetDateTime("ReturnDate"),
        //        FineAmount = reader.IsDBNull("FineAmount") ? (decimal?)null : reader.GetDecimal("FineAmount"),
        //        IsReturned = reader.GetBoolean("IsReturned"),
        //        BookTitle = reader.GetString("BookTitle"),
        //        BookAuthor = reader.GetString("BookAuthor"),
        //        MemberName = reader.GetString("MemberName"),
        //        MemberMobile = reader.GetString("MemberMobile")
        //    };
        //}
    }
}
