using Microsoft.Data.SqlClient;
using System.Data;
using WIZWEB.DAL;

namespace WIZWEB.Methods
{
    public class Method
    {
        public static void Insertresponse(string response, string customerType, int output, int status, string msg)
        {
            using SqlConnection con = BAL.GetSqlconnection();
            string query = "SP_wizwebapiresponce";

            using SqlCommand cmd = new SqlCommand(query, con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@APIResponce", response);
            cmd.Parameters.AddWithValue("@APIResponcetype", customerType);
            cmd.Parameters.AddWithValue("@RequestId", output);
            cmd.Parameters.AddWithValue("@Statuscode", status);
            cmd.Parameters.AddWithValue("@Statusmessage", msg);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void Insertresponse(string response, string customerType, string vou_number, int officllocation, int output, int status, string msg)
        {
            using SqlConnection con = BAL.GetSqlconnection();
            string query = "SP_wizwebapiresponce";

            using SqlCommand cmd = new SqlCommand(query, con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@APIResponce", response);
            cmd.Parameters.AddWithValue("@APIResponcetype", customerType);
            cmd.Parameters.AddWithValue("@vou_number", vou_number);
            cmd.Parameters.AddWithValue("@officllocation", officllocation);
            cmd.Parameters.AddWithValue("@RequestId", output);
            cmd.Parameters.AddWithValue("@Statuscode", status);
            cmd.Parameters.AddWithValue("@Statusmessage", msg);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void InsertResponce(string Responce, string APIResponcetype, string vou_number, int officllocation, int output, int statuscode, string statusmessage, string FTPfilename)
        {
            BAL SqlHelper = new BAL();
            string query7 = "SP_wizwebapiresponce";

            Dictionary<string, object> objEst7 = new Dictionary<string, object>();
            objEst7.Add("@APIResponcetype", APIResponcetype);
            objEst7.Add("@APIResponce", Responce);
            objEst7.Add("@vou_number", vou_number);
            objEst7.Add("@officllocation", officllocation);
            objEst7.Add("@RequestId", output);
            objEst7.Add("@Statuscode", statuscode);
            objEst7.Add("@Statusmessage", statusmessage);
            objEst7.Add("@FTPfilename", FTPfilename);
            bool n = SqlHelper.ExecuteNonQuery(query7, objEst7);

        }
    }

}
