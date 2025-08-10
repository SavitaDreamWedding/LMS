using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Models;

namespace Library.Core.Repositories
{
    public interface IBookMasterRepository
    {
        //List<BookMaster> GetAllBooks();
        //BookMaster GetBookById(int bookId);
        //void AddBook(BookMaster book);
        //void UpdateBook(BookMaster book);
        //void DeleteBook(int bookId);
        //void UpdateAvailableCopies(int bookId, int newAvailableCopies);
        List<BookMaster> GetAllBooks();
        BookMaster GetBookById(int bookId);
        int AddBook(BookMaster book);
        bool UpdateBook(BookMaster book);
        bool DeleteBook(int bookId);
        bool UpdateAvailableCopies(int bookId, int copies);
        List<BookMaster> GetAvailableBooks();
    }
}
