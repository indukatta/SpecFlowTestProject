using System;

namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentRequest
    {
        private string creditorAccountNumber;
        private string debitorAccountNumber;
        private decimal amount;
        private DateTime paymentDate;
        private PaymentScheme paymentScheme;
        public MakePaymentRequest(string creditorAccountNumber,
                                string debitorAccountNumber,
                                decimal amount,
                                DateTime paymentDate,
                                PaymentScheme paymentScheme){
            CreditorAccountNumber = creditorAccountNumber;
            DebtorAccountNumber = debitorAccountNumber;
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentScheme = paymentScheme;
        }

        public string CreditorAccountNumber {
            get { return creditorAccountNumber; }
            private set { creditorAccountNumber = value; }
        }

        public string DebtorAccountNumber {
            get { return debitorAccountNumber; }
            private set { debitorAccountNumber = value; }
        }

        public decimal Amount {
            get { return amount; }
            private set { amount = value; }
        }

        public DateTime PaymentDate {
            get { return paymentDate; }
            private set { paymentDate = value; }
        }

        public PaymentScheme PaymentScheme {
            get { return paymentScheme; }
            private set { paymentScheme = value; }
        }
    }
}
