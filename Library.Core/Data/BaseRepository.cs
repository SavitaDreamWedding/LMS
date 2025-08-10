using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Data
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        public BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Librarydb"].ConnectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
