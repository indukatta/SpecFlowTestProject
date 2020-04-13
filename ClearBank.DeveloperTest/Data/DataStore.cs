using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClearBank.DeveloperTest.Data
{
    public static class DataStore
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
                  "AttachDbFilename="+ path + 
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

        private static void CloseDataBaseConnection()
        {
            connection.Close();
        }

        private static void RunUpdateQuery(String query)
        {
            try
            {
                SetUpDBConnection();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        public static void updateAccountBalance(String accountNumber, decimal balance)
        {
            String query = "Update AccountDetails set Balance = "+ balance +" where AccountNumber = '" + accountNumber + "'";
            RunUpdateQuery(query);
        }
    }
}
