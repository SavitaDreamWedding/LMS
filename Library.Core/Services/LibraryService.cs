using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Library.Core.Models;
using Library.Core.Models.ViewModels;
using Library.Core.Repositories;
using Umbraco.Core.Persistence.Repositories;

namespace Library.Core.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookMasterRepository _bookRepository;
        private readonly IMemberMasterRepository _memberRepository;
        private readonly IBookIssueRepository _bookIssueRepository;

        public LibraryService(IBookMasterRepository bookRepository, IMemberMasterRepository memberRepository, IBookIssueRepository bookIssueRepository)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _bookIssueRepository = bookIssueRepository;
        }

        public ApiResponse<List<BookMaster>> GetAllBooks()
        {
            try
            {
                var books = _bookRepository.GetAllBooks();
                return ApiResponse<List<BookMaster>>.SuccessResult(books);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BookMaster>>.ErrorResult($"Error retrieving books: {ex.Message}");
            }
        }

        public ApiResponse<BookMaster> GetBookById(int bookId)
        {
            try
            {
                var book = _bookRepository.GetBookById(bookId);
                if (book == null)
                    return ApiResponse<BookMaster>.ErrorResult("Book not found");

                return ApiResponse<BookMaster>.SuccessResult(book);
            }
            catch (Exception ex)
            {
                return ApiResponse<BookMaster>.ErrorResult($"Error retrieving book: {ex.Message}");
            }
        }

        public ApiResponse<int> AddBook(BookMaster book)
        {
            try
            {
                var bookId = _bookRepository.AddBook(book);
                return ApiResponse<int>.SuccessResult(bookId, "Book added successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.ErrorResult($"Error adding book: {ex.Message}");
            }
        }

        public ApiResponse<bool> UpdateBook(BookMaster book)
        {
            try
            {
                var result = _bookRepository.UpdateBook(book);
                return result
                    ? ApiResponse<bool>.SuccessResult(true, "Book updated successfully")
                    : ApiResponse<bool>.ErrorResult("Failed to update book");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error updating book: {ex.Message}");
            }
        }

        public ApiResponse<bool> DeleteBook(int bookId)
        {
            try
            {
                var result = _bookRepository.DeleteBook(bookId);
                return result
                    ? ApiResponse<bool>.SuccessResult(true, "Book deleted successfully")
                    : ApiResponse<bool>.ErrorResult("Failed to delete book");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error deleting book: {ex.Message}");
            }
        }

        public ApiResponse<List<MemberMaster>> GetAllMembers()
        {
            try
            {
                var members = _memberRepository.GetAllMembers();
                return ApiResponse<List<MemberMaster>>.SuccessResult(members);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MemberMaster>>.ErrorResult($"Error retrieving members: {ex.Message}");
            }
        }

        public ApiResponse<MemberMaster> GetMemberById(int memberId)
        {
            try
            {
                var member = _memberRepository.GetMemberById(memberId);
                if (member == null)
                    return ApiResponse<MemberMaster>.ErrorResult("Member not found");

                return ApiResponse<MemberMaster>.SuccessResult(member);
            }
            catch (Exception ex)
            {
                return ApiResponse<MemberMaster>.ErrorResult($"Error retrieving member: {ex.Message}");
            }
        }

        public ApiResponse<int> AddMember(MemberMaster member)
        {
            try
            {
                var memberId = _memberRepository.AddMember(member);
                return ApiResponse<int>.SuccessResult(memberId, "Member added successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.ErrorResult($"Error adding member: {ex.Message}");
            }
        }

        public ApiResponse<bool> UpdateMember(MemberMaster member)
        {
            try
            {
                var result = _memberRepository.UpdateMember(member);
                return result
                    ? ApiResponse<bool>.SuccessResult(true, "Member updated successfully")
                    : ApiResponse<bool>.ErrorResult("Failed to update member");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error updating member: {ex.Message}");
            }
        }

        public ApiResponse<bool> DeleteMember(int memberId)
        {
            try
            {
                var result = _memberRepository.DeleteMember(memberId);
                return result
                    ? ApiResponse<bool>.SuccessResult(true, "Member deleted successfully")
                    : ApiResponse<bool>.ErrorResult("Failed to delete member");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error deleting member: {ex.Message}");
            }
        }

        public ApiResponse<int> IssueBook(int bookId, int memberId)
        {
            try
            {
                var book = _bookRepository.GetBookById(bookId);
                if (book == null)
                    return ApiResponse<int>.ErrorResult("Book not found");

                if (book.AvailableCopies <= 0)
                    return ApiResponse<int>.ErrorResult("Book is not available for issue");

                var bookIssue = new BookIssue
                {
                    BookId = bookId,
                    MemberId = memberId,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7)
                };

                var issueId = _bookIssueRepository.IssueBook(bookIssue);
                return ApiResponse<int>.SuccessResult(issueId, "Book issued successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.ErrorResult($"Error issuing book: {ex.Message}");
            }
        }

        public ApiResponse<bool> ReturnBook(int issueId)
        {
            try
            {
                var issue = _bookIssueRepository.GetIssueById(issueId);
                if (issue == null)
                    return ApiResponse<bool>.ErrorResult("Issue record not found");

                if (issue.IsReturned)
                    return ApiResponse<bool>.ErrorResult("Book is already returned");

                var fineAmount = CalculateFine(issue.DueDate, DateTime.Now);
                var result = _bookIssueRepository.ReturnBook(issueId, fineAmount > 0 ? fineAmount : (decimal?)null);

                return result
                    ? ApiResponse<bool>.SuccessResult(true, $"Book returned successfully. Fine: ₹{fineAmount}")
                    : ApiResponse<bool>.ErrorResult("Failed to return book");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error returning book: {ex.Message}");
            }
        }

        public ApiResponse<List<BookIssue>> GetActiveIssues()
        {
            try
            {
                var issues = _bookIssueRepository.GetActiveIssues();
                return ApiResponse<List<BookIssue>>.SuccessResult(issues);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BookIssue>>.ErrorResult($"Error retrieving active issues: {ex.Message}");
            }
        }

        public ApiResponse<List<BookIssue>> GetOverdueIssues()
        {
            try
            {
                var issues = _bookIssueRepository.GetOverdueIssues();
                return ApiResponse<List<BookIssue>>.SuccessResult(issues);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BookIssue>>.ErrorResult($"Error retrieving overdue issues: {ex.Message}");
            }
        }

        public ApiResponse<List<BookIssue>> GetBookHistory(int bookId)
        {
            try
            {
                var history = _bookIssueRepository.GetBookHistory(bookId);
                return ApiResponse<List<BookIssue>>.SuccessResult(history);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BookIssue>>.ErrorResult($"Error retrieving book history: {ex.Message}");
            }
        }

        public ApiResponse<DashboardViewModel> GetDashboardData()
        {
            try
            {
                var dashboard = _bookIssueRepository.GetDashboardData();
                return ApiResponse<DashboardViewModel>.SuccessResult(dashboard);
            }
            catch (Exception ex)
            {
                return ApiResponse<DashboardViewModel>.ErrorResult($"Error retrieving dashboard data: {ex.Message}");
            }
        }

        public List<SelectListItem> GetBookDropdownList()
        {
            try
            {
                var books = _bookRepository.GetAvailableBooks();
                return books.Select(b => new SelectListItem
                {
                    Value = b.BookId.ToString(),
                    Text = $"{b.Title} - {b.Author} (Available: {b.AvailableCopies})"
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        public List<SelectListItem> GetMemberDropdownList()
        {
            try
            {
                var members = _memberRepository.GetActiveMembers();
                return members.Select(m => new SelectListItem
                {
                    Value = m.MemberId.ToString(),
                    Text = $"{m.FullName} - {m.Mobile}"
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        public decimal CalculateFine(DateTime dueDate, DateTime returnDate)
        {
            if (returnDate <= dueDate)
                return 0;

            var overdueDays = (returnDate - dueDate).Days;
            return overdueDays * 10; // ₹10 per day
        }
    }
}
