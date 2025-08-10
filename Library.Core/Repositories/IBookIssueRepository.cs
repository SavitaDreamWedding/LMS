using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Core.Models.ViewModels;

namespace Library.Core.Repositories
{
    
    public interface IBookIssueRepository
    {
        List<BookIssue> GetAllIssues();
        List<BookIssue> GetActiveIssues();
        List<BookIssue> GetOverdueIssues();
        BookIssue GetIssueById(int issueId);
        List<BookIssue> GetBookHistory(int bookId);
        List<BookIssue> GetMemberHistory(int memberId);
        int IssueBook(BookIssue bookIssue);
        bool ReturnBook(int issueId, decimal? fineAmount);
        List<ChartData> GetIssuesPerDay(int days);
        List<ChartData> GetBooksByCategory();
        DashboardViewModel GetDashboardData();
    }
}
