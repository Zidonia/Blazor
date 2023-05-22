using Eko.InvoiceSearch.Shared.Seguridad;
using System.Data;
using System.Data.SqlClient;

namespace Eko.InvoiceSearch.Server.Data
{
    public class ReUseSQL
    {
        public static SQLConnConfig _conn;
        public SqlConnection connection;

        public ReUseSQL(SQLConnConfig conn)
        {
			Seguridad mSeguridad = new Seguridad();
			_conn = conn;
            connection = new SqlConnection(mSeguridad.DesencriptarDator(_conn.Value));
        }
        public async Task SaveReord(string sqlStr)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlStr, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public DataTable GetRecord(string sqlStr)
        {
            SqlCommand cmd = new SqlCommand(sqlStr, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetDataTableStoredProcedure(string storedProcedure, SqlParameter[] listaParametros)
        {
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(storedProcedure, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(listaParametros);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                connection.Close();
                da.Fill(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }
    }
}
