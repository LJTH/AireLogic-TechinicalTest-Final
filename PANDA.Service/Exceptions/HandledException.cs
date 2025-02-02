namespace PANDA.Service.Exceptions
{
    public class HandledException : Exception
    {
        public int StatusCode { get; }
        public HandledException(string message, int statusCode) :base(message) 
        { 
            StatusCode = statusCode;
        }
    }
}
