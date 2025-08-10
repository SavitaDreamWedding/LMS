using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveIssues { get; set; }
        public int OverdueBooks { get; set; }
        public List<ChartData> IssuesPerDay { get; set; }
        public List<ChartData> BooksByCategory { get; set; }
    }
    public class ChartData
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}
