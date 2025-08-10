using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Library.Core.Models;
using Library.Core.Models.ViewModels;

namespace Library.Core.Services
{
    public interface ILibraryService1
    {
        // Book operations
        ApiResponse<List<BookMaster>> GetAllBooks();
        ApiResponse<BookMaster> GetBookById(int bookId);
        ApiResponse<int> AddBook(BookMaster book);
        ApiResponse<bool> UpdateBook(BookMaster book);
        ApiResponse<bool> DeleteBook(int bookId);

        // Member operations
        ApiResponse<List<MemberMaster>> GetAllMembers();
        ApiResponse<MemberMaster> GetMemberById(int memberId);
        ApiResponse<int> AddMember(MemberMaster member);
        ApiResponse<bool> UpdateMember(MemberMaster member);
        ApiResponse<bool> DeleteMember(int memberId);

        // Issue operations
        ApiResponse<int> IssueBook(int bookId, int memberId);
        ApiResponse<bool> ReturnBook(int issueId);
        ApiResponse<List<BookIssue>> GetActiveIssues();
        ApiResponse<List<BookIssue>> GetOverdueIssues();
        ApiResponse<List<BookIssue>> GetBookHistory(int bookId);

        // Dashboard
        ApiResponse<DashboardViewModel> GetDashboardData();

        // Dropdown data
        List<SelectListItem> GetBookDropdownList();
        List<SelectListItem> GetMemberDropdownList();

        // Calculate fine
        decimal CalculateFine(DateTime dueDate, DateTime returnDate);
    }

    public interface ILibraryService
    {
        // Book operations
        ApiResponse<List<BookMaster>> GetAllBooks();
        ApiResponse<BookMaster> GetBookById(int bookId);
        ApiResponse<int> AddBook(BookMaster book);
        ApiResponse<bool> UpdateBook(BookMaster book);
        ApiResponse<bool> DeleteBook(int bookId);

        // Member operations
        ApiResponse<List<MemberMaster>> GetAllMembers();
        ApiResponse<MemberMaster> GetMemberById(int memberId);
        ApiResponse<int> AddMember(MemberMaster member);
        ApiResponse<bool> UpdateMember(MemberMaster member);
        ApiResponse<bool> DeleteMember(int memberId);

        // Issue operations
        ApiResponse<int> IssueBook(int bookId, int memberId);
        ApiResponse<bool> ReturnBook(int issueId);
        ApiResponse<List<BookIssue>> GetActiveIssues();
        ApiResponse<List<BookIssue>> GetOverdueIssues();
        ApiResponse<List<BookIssue>> GetBookHistory(int bookId);

        // Dashboard
        ApiResponse<DashboardViewModel> GetDashboardData();

        // Dropdown data
        List<SelectListItem> GetBookDropdownList();
        List<SelectListItem> GetMemberDropdownList();

        // Calculate fine
        decimal CalculateFine(DateTime dueDate, DateTime returnDate);
    }
}
