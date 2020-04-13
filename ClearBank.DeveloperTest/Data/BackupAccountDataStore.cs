using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System;

namespace ClearBank.DeveloperTest.Data
{
    public class BackupAccountDataStore
    {
        public Account GetAccount(string accountNumber)
        {
            // Access backup data base to retrieve account, code removed for brevity 
            Dictionary<String, Object> accountDetails = DataStore.GetAccountDetails(accountNumber);
            string accountNo = accountDetails["AccountNumber"].ToString();
            decimal balance = decimal.Parse(accountDetails["Balance"].ToString());
            AccountStatus status = (AccountStatus)accountDetails["AccountStatus"];
            AllowedPaymentSchemes allowedPaymentSchemes = (AllowedPaymentSchemes)accountDetails["AllowedPaymentSchemes"];
            return new Account(accountNo, balance, status, allowedPaymentSchemes);
        }

        public void UpdateAccount(Account account)
        {
            // Update account in backup database, code removed for brevity
            DataStore.updateAccountBalance(account.AccountNumber, account.Balance);
        }
    }
}
