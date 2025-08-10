using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Models
{
    public class BookIssue
    {
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Book is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Member is required")]
        public int MemberId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal? FineAmount { get; set; }

        public bool IsReturned { get; set; }

        // Navigation properties for display
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string MemberName { get; set; }
        public string MemberMobile { get; set; }
    }
}
