namespace PANDA.WebApi.ExceptionHandling
{
    public class ErrorResponse
    {
        internal int HttpStatusCode { get; set; }
        public string ErrorDetails { get; set; } = string.Empty;
        public string ExceptionMessage { get; set; } = string.Empty;
        public bool IsUnhandledException { get; internal set; }
    }
}
