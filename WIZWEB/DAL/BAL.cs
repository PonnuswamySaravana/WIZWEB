using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace WIZWEB.DAL
{

    public class BAL
    {

        // public static string link = "https://wiz.ifact.co.in/apitestlive/";
        //public static string link = "https://wiz.ifact.co.in/radar/";
        //public static string link = "http://localhost:52702/";
        public static string link = "https://ltsolutions.ifact.co.in/elyxrapitestlive/";
        public static string source = "WLSPTIntegrtn";


        private static string connectionstring= "Data Source=sqlserverlt.privatelink.database.windows.net;Initial Catalog=IFT_ELYXR_FC;User ID=ltadmin;pwd=@)!#eroC@)@!; TrustServerCertificate=True;";
        

        public static SqlConnection GetSqlconnection()
        {
            return new SqlConnection(connectionstring);
        }

        public bool ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(procedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
        internal DataTable ExecuteTable1(string storedProcedureName, Dictionary<string, object> parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(storedProcedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        public string ExecuteNonQueryPara(string query, Dictionary<string, object> details)
        {
            string output = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var item in details)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value ?? DBNull.Value);
                }

                cmd.Parameters.Add("@eid", SqlDbType.Int).Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                int eid = Convert.ToInt32(cmd.Parameters["@eid"].Value);
                output = eid.ToString();
            }

            return output;
        }

        public string ExecuteNonQueryPara_sara(string query, Dictionary<string, object> details)
        {
            string output = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand sqlCommand = new SqlCommand(query, conn))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                foreach (var item in details)
                {
                    sqlCommand.Parameters.AddWithValue(item.Key, item.Value ?? DBNull.Value);
                }

                SqlParameter outputParam = new SqlParameter("@refno", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(outputParam);

                conn.Open();
                sqlCommand.ExecuteNonQuery();

                if (outputParam.Value != DBNull.Value)
                {
                    int refNo = Convert.ToInt32(outputParam.Value);
                    output = refNo.ToString();
                }
            }

            return output;
        }



    }



}
