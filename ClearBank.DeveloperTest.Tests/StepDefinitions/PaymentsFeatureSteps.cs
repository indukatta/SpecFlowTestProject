using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace ClearBank.DeveloperTest.Tests
{
    [Binding]
    public sealed class PaymentsFeatureSteps : Steps
    {
        private readonly ScenarioContext context;
        
        /* Using scenario context via dependency injection */
        public PaymentsFeatureSteps(ScenarioContext injectedContext)
        {
            context = injectedContext;
        }

        /* Retrive the account details from database */
        [Given("I have retrieved (.*) account details from the database")]
        public void GetAccountDetailsFromDB(String accountType)
        {
            if (accountType.Equals("debitor")) { 
                Dictionary<String, Object> accountDetails = DataSQLFunctions.GetRandomAccountDetails("Live");
                context["DebitorAccount"] = accountDetails;
                Console.WriteLine("Debitor account retrieved from database : [" + accountDetails["AccountNumber"].ToString() + "]");
            }
            else if (accountType.Equals("creditor"))
            {
                Dictionary<String, Object> accountDetails = DataSQLFunctions.GetRandomAccountDetails("InboundPaymentsOnly");
                context["CreditorAccount"] = accountDetails;
                Console.WriteLine("Creditor account retrieved from database : [" + accountDetails["AccountNumber"].ToString() + "]");
            }
        }

        [When("I have entered (.*) debit amount")]
        public void EnterDebitAmount(decimal number)
        {
            context["DebitAmount"] = number;
        }

        /* Call make payment service */
        [When("Make a payment request")]
        public void MakePaymentRequest()
        {
            Dictionary<String, Object> debitorAccount = (Dictionary<String, Object>)context["DebitorAccount"];
            Dictionary<String, Object> creditorAccount = (Dictionary<String, Object>)context["CreditorAccount"];
            DateTime paymentDate = DateTime.Now;

            string creditorAccountNumber = creditorAccount["AccountNumber"].ToString();
            string debitorAccountNumber = debitorAccount["AccountNumber"].ToString();
           
            decimal debitAmount = (decimal)context["DebitAmount"];

           // Object status = Enum.Parse(typeof(AccountStatus), debitorAccount["AccountStatus"].ToString());
            PaymentScheme paymentScheme = (PaymentScheme)Enum.Parse(typeof(PaymentScheme), debitorAccount["PaymentScheme"].ToString());

            MakePaymentRequest request = new MakePaymentRequest(creditorAccountNumber,
            debitorAccountNumber, debitAmount, paymentDate, paymentScheme);

            PaymentService paymentService = new PaymentService();
            MakePaymentResult result  = paymentService.MakePayment(request);
            context["MakePaymentResult"] = result;
        }

        /* Call make payment service with invalid payment scheme */
        [When("Make a payment request with invalid payment scheme")]
        public void MakePaymentRequestWithInvalidPayementScheme()
        {
            Dictionary<String, Object> debitorAccount = (Dictionary<String, Object>)context["DebitorAccount"];
            Dictionary<String, Object> creditorAccount = (Dictionary<String, Object>)context["CreditorAccount"];
            DateTime paymentDate = DateTime.Now;

            string creditorAccountNumber = creditorAccount["AccountNumber"].ToString();
            string debitorAccountNumber = debitorAccount["AccountNumber"].ToString();

            decimal debitAmount = (decimal)context["DebitAmount"];

            // Object status = Enum.Parse(typeof(AccountStatus), debitorAccount["AccountStatus"].ToString());
            PaymentScheme debitorPaymentScheme = (PaymentScheme)Enum.Parse(typeof(PaymentScheme), debitorAccount["PaymentScheme"].ToString());

            //Get invalid payment scheme
            var values = Enum.GetValues(typeof(PaymentScheme));
            foreach (var val in values)
            {
                if (!(val.ToString().Equals(debitorAccount["PaymentScheme"].ToString())))
                {
                    debitorPaymentScheme = (PaymentScheme)Enum.Parse(typeof(PaymentScheme), val.ToString());
                    break;
                }
            }

            MakePaymentRequest request = new MakePaymentRequest(creditorAccountNumber,
                debitorAccountNumber, debitAmount, paymentDate, debitorPaymentScheme);

            PaymentService paymentService = new PaymentService();
            MakePaymentResult result = paymentService.MakePayment(request);
            context["MakePaymentResult"] = result;
        }

        /* Verify if payment is successful/ not successful */
        [Then("Verify payment has been made : (.*)")]
        public void VerifyPaymentResult(bool expectedResult)
        {
            MakePaymentResult actualResult = (MakePaymentResult)context["MakePaymentResult"];
            Assert.AreEqual(expectedResult, actualResult.Success,"Verify expected payment result");
            Console.WriteLine("Make payment expected result : [" + expectedResult +"], actual result : [" + actualResult.Success + "]");
        }

        /* Check if amount is deducted from debitor account */
        [Then("Verify debitor account balance after payment")]
        public void VerifyPostDebitAccountBalance()
        {
            Dictionary<String, Object> debitorAccount = (Dictionary<String, Object>)context["DebitorAccount"];
            string debitorAccountNumber = debitorAccount["AccountNumber"].ToString();
            decimal initialDebitorBalance = decimal.Parse(debitorAccount["Balance"].ToString());
            decimal debitAmount = (decimal)context["DebitAmount"];

            Dictionary<String, Object> postDebitorAccountDetails = DataSQLFunctions.GetAccountDetails(debitorAccountNumber);
            decimal postDebitorBalance = decimal.Parse(postDebitorAccountDetails["Balance"].ToString());


            Assert.AreEqual(initialDebitorBalance - debitAmount, postDebitorBalance,"Verify amount["+ debitAmount +"] deducted from debitor account");
            Console.WriteLine("Expected account balance = initialDebitorBalance ["+ initialDebitorBalance + "] - " +
                "debitAmount ["+debitAmount +"] = ["+ (initialDebitorBalance- debitAmount) + "]");
            Console.WriteLine("Actual account balance after debit = ["+ postDebitorBalance + "]");
        }

        /* Check if amount is deducted from debitor account */
        [Then("Verify debitor account balance after payment transaction fail")]
        public void VerifyBalanceWhenAmountNotDeducted()
        {
            Dictionary<String, Object> debitorAccount = (Dictionary<String, Object>)context["DebitorAccount"];
            string debitorAccountNumber = debitorAccount["AccountNumber"].ToString();
            decimal initialDebitorBalance = decimal.Parse(debitorAccount["Balance"].ToString());
            //decimal debitAmount = (decimal)context["DebitAmount"];

            Dictionary<String, Object> postDebitorAccountDetails = DataSQLFunctions.GetAccountDetails(debitorAccountNumber);
            decimal postDebitorBalance = decimal.Parse(postDebitorAccountDetails["Balance"].ToString());


            Assert.AreEqual(initialDebitorBalance, postDebitorBalance, "Verify debitor account balance remains same when payment transaction fails.");
            Console.WriteLine("Initial account balance = [" + initialDebitorBalance + "], post payment transaction fail balance = [" + postDebitorBalance + "]");
        }
    }
}
