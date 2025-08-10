using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Library.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_Error(object sender, EventArgs e)
        {
            // Get the exception that caused the error
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                // Log the error
                LogError(exception);
            }
        }
        private void LogError(Exception exception)
        {
            try
            {
                string logDirectory = Server.MapPath("~/logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string logFilePath = Path.Combine(logDirectory, "errors.txt");
                string logEntry = string.Format(
                    "[{0}] {1}: {2}\nStack Trace: {3}\n{4}\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    exception.GetType().Name,
                    exception.Message,
                    exception.StackTrace,
                    new string('-', 80)
                );

                File.AppendAllText(logFilePath, logEntry);
            }
            catch
            {
                // Ignore logging errors to prevent infinite loops
            }
        }
    }
}
