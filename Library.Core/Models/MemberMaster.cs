using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Models
{
    public class MemberMaster
    {
        public int MemberId { get; set; }
        
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(255, ErrorMessage = "Full Name cannot exceed 255 characters")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Mobile is required")]
        [StringLength(15, ErrorMessage = "Mobile cannot exceed 15 characters")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile must be 10 digits")]
        public string Mobile { get; set; }
        
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255)]
        public string Email { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
