using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace library_app
{

    public interface IDatabaseConnection
    {
        void OpenConnection();
        void CloseConnection();
        SqlConnection GetConnection();
    }

    public interface IDatabaseQueryExecutor
    {
        DataTable ExecuteQuery(string query);
    }
    public class SqlDatabaseConnection : IDatabaseConnection
    {
        private SqlConnection _connection;

        public SqlDatabaseConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryAppDB"].ConnectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }
    }

   public class SqlDatabaseQueryExecutor : IDatabaseQueryExecutor
    {
        private readonly IDatabaseConnection _databaseConnection;

        public SqlDatabaseQueryExecutor(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                _databaseConnection.OpenConnection();
                using (SqlCommand cmd = new SqlCommand(query, _databaseConnection.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _databaseConnection.CloseConnection();
            }
            return dt;
        }
    }

}
