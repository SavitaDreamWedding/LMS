using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Library.Core.Models.ViewModels
{
    public class IssueBookViewModel
    {
        public BookIssue BookIssue { get; set; }
        public List<SelectListItem> Books { get; set; }
        public List<SelectListItem> Members { get; set; }
    }
}
