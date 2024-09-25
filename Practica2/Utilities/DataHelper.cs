using Practica2.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace Practica2.Utilities
{
    public class DataHelper
    {


            private static DataHelper _instancia;
            private SqlConnection _connection;

            private DataHelper()
            {
                _connection = new SqlConnection(Properties.Resources.cnn);
            }

            public static DataHelper GetInstance()
            {
                if (_instancia == null)
                    _instancia = new DataHelper();

                return _instancia;
            }

            public SqlConnection GetConnection()
            { 
                return _connection; 
            }

            public DataTable ExecuteSPQuery(string sp, List<ParameterSQL>? parametros)
            {
                DataTable t = new DataTable();
                SqlCommand cmd = null;

                try
                {
                    _connection.Open();
                    cmd = new SqlCommand(sp, _connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parametros != null)
                    {
                        foreach (var param in parametros)
                            cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }

                    t.Load(cmd.ExecuteReader());
                }
                catch (SqlException)
                {
                    t = null;
                }
                finally
                {
                    if (_connection.State == System.Data.ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                }

                return t;
            }



            public int ExecuteSPDML(string sp, List<ParameterSQL>? parametros)
            {
                int rows;
                try
                {
                    _connection.Open();
                    var cmd = new SqlCommand(sp, _connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (parametros != null)
                    {
                        foreach (var param in parametros)
                            cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }

                    rows = cmd.ExecuteNonQuery();
                    _connection.Close();
                }
                catch (SqlException)
                {
                    rows = 0;
                }
                finally
                {
                    if (_connection.State == System.Data.ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                }

                return rows;
            }

    }
}
