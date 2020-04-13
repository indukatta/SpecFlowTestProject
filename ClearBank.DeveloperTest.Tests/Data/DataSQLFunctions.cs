using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Data
{
    public static class DataSQLFunctions
    {
        private static SqlConnection connection;
        private static void SetUpDBConnection()
        {
            try
            {
                Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).ToString();
                String path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).ToString()).ToString() + "\\ClearBank.DeveloperTest\\CustomerDataBase\\Database1.mdf;";
                string connectionString =
                  "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                  "AttachDbFilename=" + path +
                  "Integrated Security = True";

                // Create and open the DB connection 
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static SqlDataReader ExecuteQuery(String query)
        {
            SetUpDBConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reader;
        }

        public static Dictionary<String, Object> GetAccountDetails(string accountNumber)
        {
            Dictionary<String, Object> accountDetails = new Dictionary<string, Object>();
            String query = "Select AccountNumber, Balance, AccountStatus, PaymentScheme from AccountDetails where AccountNumber = '" + accountNumber + "'";
            SqlDataReader reader = ExecuteQuery(query);

            if (reader.HasRows == false)
            {
                throw new Exception("Account [" + accountNumber + "] not found in the databse");
            }

            while (reader.Read())
            {
                accountDetails.Add("AccountNumber", reader["AccountNumber"]);
                accountDetails.Add("Balance", reader["Balance"]);
                accountDetails.Add("AccountStatus", reader["AccountStatus"]);
                accountDetails.Add("PaymentScheme", reader["PaymentScheme"]);
            }
            return accountDetails;
        }

        /**
         * @Input accountStatusType : Live, InboundPaymentsOnly, Disabled
         */
        public static Dictionary<String, Object> GetRandomAccountDetails(string accountStatusType)
        {
            Dictionary<String, Object> accountDetails = new Dictionary<string, Object>();
            String query = "select top 1 AccountNumber, Balance, AccountStatus, PaymentScheme from AccountDetails where AccountStatus in ('" + accountStatusType + "') order by newid()";
            SqlDataReader reader = ExecuteQuery(query);

            if (reader.HasRows == false)
            {
                throw new Exception("No accounts found in the databse with account status type [" + accountStatusType + "]");
            }

            while (reader.Read())
            {
                accountDetails.Add("AccountNumber", reader["AccountNumber"]);
                accountDetails.Add("Balance", reader["Balance"]);
                accountDetails.Add("AccountStatus", reader["AccountStatus"]);
                accountDetails.Add("PaymentScheme", reader["PaymentScheme"]);
            }
            return accountDetails;
        }
    }
}
