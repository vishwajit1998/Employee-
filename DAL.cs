using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp3
{
    public class DAL
    {

        public bool isProcCall = false;
        List<SqlParameter> paralist = new List<SqlParameter>();


        // public ConnectionState State = 
        //ConnectionState.Closed;

        public SqlConnection GetConnection()
        {
           // string ConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
            SqlConnection con = new SqlConnection();
            con.ConnectionString= @"Data Source=SHRADDHA\SQLEXPRESS;Initial Catalog=dbaugust;Integrated Security=True";
            //con.ConnectionString = ConnectionString;
            return con;

        }
        public void ClearParameters()
        {
            paralist.Clear();
        }
        public void AddParameters(string paraname, string value)
        {
            paralist.Add(new SqlParameter(paraname, value));
        }
        private SqlCommand GetCommand(string Query)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            if (isProcCall)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(paralist.ToArray());
            }
            else

                cmd.CommandType = CommandType.Text;

            cmd.CommandText = Query;
            cmd.Connection = GetConnection();
            return cmd;

        }

        private DataSet GetTables(String Query)
        {
            SqlDataAdapter da = new SqlDataAdapter(GetCommand(Query));
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        private DataTable GetTable(String Query)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = GetCommand(Query);
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr != null && rdr.HasRows)
                dt.Load(rdr);
            cmd.Connection.Close();
            return dt;

        }
        public object GetID(String Query)
        {
            SqlCommand cmd = GetCommand(Query);
            cmd.Connection.Open();

            object retval = cmd.ExecuteScalar();
            cmd.Connection.Close();

            return retval;
        }

        public SqlDataReader GetDataReader(String Query)
        {
            SqlCommand cmd = GetCommand(Query);
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return rdr;
        }

        public int ExecuteQuery(String Query)
        {
            SqlCommand cmd = GetCommand(Query);
            cmd.Connection.Open();

            int retval = cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return retval;
        }

    }
}

