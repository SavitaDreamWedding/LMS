Library Management Portal (LMS Lite)
 A comprehensive Library Management System built with ASP.NET MVC 5, ADO.NET, and modern web
 technologies.
 🛠
 Tech Stack
 Backend: ASP.NET MVC 5 (.NET Framework 4.8)
 Database: SQL Server with ADO.NET (No ORM)
 Frontend: Bootstrap 5, jQuery, Chart.js
 Architecture: Clean separation with Library.Core and Library.Web projects
 📦
 Features
 Master Modules
 Books Management: CRUD operations for books with real-time availability tracking
 Members Management: Complete member lifecycle management
 AJAX-powered forms with client and server-side validation
 Transaction Module
 Issue Books: Smart book issuing with availability checks
 Return Books: Automatic fine calculation for overdue returns
 Business Rules: 7-day lending period, ₹10/day late fee
 Dashboard & Reports
 Interactive Dashboard: Real-time statistics and charts
 Overdue Members Report: Track and manage late returns
 Book History: Complete transaction history per book
 Visual Analytics: Chart.js powered insights
 🚀
 Setup Instructions
 Prerequisites
 Visual Studio 2019/2022
 SQL Server (LocalDB or Full SQL Server)
 .NET Framework 4.8
Installation Steps
 1. Create the Solution Structure
 LibraryManagement/
 ├── Library.Core/
 │   ├── Models/
 │   ├── Repositories/
 │   ├── Services/
 │   └── Data/
 └── Library.Web/
 ├── Controllers/
 ├── Views/
 ├── Scripts/
 └── Content/
 2. Database Setup
 Execute the 
library_schema.sql script in SQL Server Management Studio
 Update connection string in 
Web.config if needed
 Default connection uses LocalDB
 3. Project References
 Library.Web references Library.Core
 Install NuGet packages:
 Microsoft.AspNet.Mvc (5.2.9)
 Bootstrap (5.3.0)
 jQuery (3.6.0)
 4. Configuration
 Update 
Web.config connection string
 Ensure 
customErrors mode="Off" for development
 Configure session timeout as needed
 🔧
 Technical Decisions
 Architecture Choices
 Two-Project Structure: Clean separation of concerns
 Repository Pattern: Encapsulates data access logic
 Service Layer: Business logic and transaction management
Pure ADO.NET: No ORM dependency, full control over SQL
 Security Features
 Session-based Authentication: Simple login system
 Parameterized Queries: SQL injection prevention
 Input Validation: Client and server-side validation
 Error Logging: Comprehensive exception logging
 Performance Optimizations
 Database Indexes: Optimized query performance
 AJAX Operations: No full page reloads
 Efficient Queries: Minimal database roundtrips
 Connection Management: Proper disposal patterns
 📊
 Sample Data
 The system comes pre-loaded with:
 10 sample books across different categories
 8 sample members with contact details
 Historical transactions including overdue scenarios
Default Credentials
 Username: 
admin
 Password: 
admin123
 Usage Examples
 Adding a New Book
 1. Navigate to Books → Add New Book
 2. Fill in title, author, ISBN, category, and copies
 3. System automatically sets available copies = total copies
 Issuing a Book
 1. Go to Issue Book page
 2. Select available book and active member
 3. System automatically calculates due date (7 days)
 4. Available copies decremented automatically
Returning a Book
 1. View Active Issues
 2. Click Return button for the relevant issue
 3. System calculates fine if overdue (₹10/day)
 4. Available copies incremented automatically
 Viewing Reports
 Dashboard: Real-time metrics and charts
 Overdue Report: All pending returns with calculated fines
 Book History: Complete transaction log per book
 Testing Scenarios
 1. Normal Flow:
 Add book with 10 copies
 Issue to member → Available copies = 9
 Return on time → No fine, Available copies = 10
 2. Overdue Scenario:
 Issue book
 Wait past due date
 Return → Fine = (Days overdue × ₹10)
 3. Validation Testing:
 Try issuing unavailable book
 Submit forms with invalid data
 Test mobile number format validation
 📁
 Project Structure
Library.Core/
 ├── Models/
 │   ├── BookMaster.cs
 │   ├── MemberMaster.cs
 │   ├── BookIssue.cs
 │   ├── User.cs
 │   ├── ApiResponse.cs
 │   └── ViewModels/
 ├── Repositories/
 │   ├── IBookRepository.cs
 │   ├── BookRepository.cs
 │   ├── IMemberRepository.cs
 │   ├── MemberRepository.cs
 │   ├── IBookIssueRepository.cs
 │   └── BookIssueRepository.cs
 ├── Services/
 │   ├── ILibraryService.cs
 │   └── LibraryService.cs
 └── Data/
 └── BaseRepository.cs
 Library.Web/
 ├── Controllers/
 │   ├── BaseController.cs
 │   ├── AccountController.cs
 │   ├── DashboardController.cs
 │   ├── BooksController.cs
 │   ├── MembersController.cs
 │   ├── IssueController.cs
 │   └── ReportsController.cs
 ├── Views/
 │   ├── Shared/
 │   ├── Account/
 │   ├── Dashboard/
 │   ├── Books/
 │   ├── Members/
 │   ├── Issue/
 │   └── Reports/
 ├── App_Start/
 │   └── RouteConfig.cs
 ├── Global.asax
 └── Web.config
 Default Credentials
 Username: 
admin
 Password: 
admin123
 UI Features
 Responsive Design: Bootstrap 5 for mobile-friendly interface
 Modern Aesthetics: Gradient backgrounds and smooth animations
 Interactive Charts: Real-time data visualization
 Modal Dialogs: Seamless CRUD operations
 Status Indicators: Color-coded badges for book availability
 Print Support: Printable reports
 Error Handling
 Global Exception Logging: All errors logged to 
logs/errors.txt
 User-Friendly Messages: Clear error communication
 Validation Feedback: Real-time form validation
 Graceful Degradation: Fallbacks for failed operations
 Dashboard Metrics
 Summary Cards: Total books, members, active issues, overdue count
 Bar Chart: Books issued per day (last 7 days)
 Pie Chart: Distribution of books by category
