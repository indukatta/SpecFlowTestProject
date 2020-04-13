namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        private string accountNumber;
        private decimal balance;
        private AccountStatus status;
        private AllowedPaymentSchemes allowedPaymentSchemes;

        public Account(string accountNumber,
            decimal balance,
            AccountStatus status,
            AllowedPaymentSchemes allowedPaymentSchemes)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            AccountStatus = status;
            AllowedPaymentSchemes = allowedPaymentSchemes;
        }

        public string AccountNumber {
            get { return accountNumber; }
            private set { accountNumber = value; }
        }

        public decimal Balance {
            get { return balance; }
            set { balance = value; }
        }

        public AccountStatus AccountStatus { get; }

        public AccountStatus Status {
            get { return status; }
            private set { status = value; }
        }

        public AllowedPaymentSchemes AllowedPaymentSchemes {
            get { return allowedPaymentSchemes; }
            private set { allowedPaymentSchemes = value; }
        }
    }
}
