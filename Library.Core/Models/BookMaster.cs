using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Models
{
    public class BookMaster
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(255, ErrorMessage = "Author cannot exceed 255 characters")]
        public string Author { get; set; }

        [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Total Copies is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Copies must be at least 1")]
        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
