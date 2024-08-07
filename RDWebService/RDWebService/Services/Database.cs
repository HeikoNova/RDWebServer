using RDWebService.Classes;
using System.Data;
using System.Data.SqlClient;

namespace RDWebService.Services
{
    public class Database
    {

        static string con;
        static string con1 = @"Server=.\SQLExpress;Database=EXAMPLE;User ID=sa;pwd=1234;Trusted_Connection=True;TrustServerCertificate=True";
        static SqlConnection? sqlcon;


        public static void setConnection()
        {
            //Credentials sollen über DLL verschlüsselt kommen
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;database=examples;Trusted_Connection=True;TrustServerCertificate=True";//TODO
            con1 = @"Server=.\SQLExpress;Database=EXAMPLE;User ID=sa;pwd=1234;Trusted_Connection=True;TrustServerCertificate=True";
            //CryptoStream
            sqlcon = new SqlConnection(con1);
            sqlcon.Open();
        }
        public static DataSet getDataFromSql(string query)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(query, con1);
            DataSet ds = new DataSet();


            try
            {
                setConnection();
                adapter.Fill(ds);

            }
            catch (SqlException err)
            {

                throw;
            }

            return ds;
        }
        public static DataTable getDataFromSqlAsTable(string query)
        {

            SqlDataAdapter adapter = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();


            try
            {

                setConnection();
                adapter.Fill(dt);

            }
            catch (SqlException err)
            {

                throw;
            }
            return dt;

        }
        public static void updateUserPw(string username, string password)
        {

            byte[] key = null;
            byte[] iv = null;
            byte[] encrypted = null;

            key = Cryptor.generateKey();
            DllHandler handler = new DllHandler();
            handler.LoadDbSettings();
            iv = Cryptor.generateIV(handler.user);

            encrypted = DllHandler.EncryptStringToBytes_Aes(password, key, iv);

            string query = "update AccessControl set password = " + encrypted + " where UserId = " + handler.user;

            //set initial false

            string query1 = "update AccessControl set pwdinitial = 0 ";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, con1);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);

            sqlDataAdapter.UpdateCommand = builder.GetUpdateCommand();

            sqlDataAdapter.UpdateCommand.ExecuteNonQuery();
        }
        public static DataSet getInitialPw()
        {

            string userName = Environment.UserName;
            string userName2 = "'" + userName + "'";
            userName = "UB0001";
            string query = "SELECT PASSWORD FROM ACCESSCONTROL WHERE USERID = " + userName2;
            setConnection();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlcon);
            //SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);

            //sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
            //TODO: Wie bekomme ich das result from dataapdater
            //  sqlDataAdapter.SelectCommand.BeginExecuteReader();

            DataSet ds = new DataSet();


            try
            {
                //setConnection();
                sqlDataAdapter.Fill(ds);

            }
            catch (SqlException err)
            {

                throw;
            }

            return ds;







        }
   

    }
}
