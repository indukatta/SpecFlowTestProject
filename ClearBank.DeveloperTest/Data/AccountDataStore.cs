using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStore
    {
        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            Dictionary<String, Object> accountDetails = DataStore.GetAccountDetails(accountNumber);
            string accountNo = accountDetails["AccountNumber"].ToString();
            decimal balance = decimal.Parse(accountDetails["Balance"].ToString());
            Object status = Enum.Parse(typeof(AccountStatus), accountDetails["AccountStatus"].ToString());
            Object paymentSchemes = Enum.Parse(typeof(AllowedPaymentSchemes), accountDetails["PaymentScheme"].ToString());
            return new Account(accountNo, balance, (AccountStatus)status, (AllowedPaymentSchemes)paymentSchemes);
        }

        public void UpdateAccount(Account account)
        {
            DataStore.updateAccountBalance(account.AccountNumber, account.Balance);
            // Update account in database, code removed for brevity
        }
    }
}
