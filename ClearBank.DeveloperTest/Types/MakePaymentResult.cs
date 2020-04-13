namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        bool status = true;
        public bool Success {
            get { return status; }
            set { status = value; }
        }
    }
}
