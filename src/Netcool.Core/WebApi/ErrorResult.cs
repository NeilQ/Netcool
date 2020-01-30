namespace Netcool.Core.WebApi
{
    public class ErrorResult
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public ErrorResult()
        {
        }

        public ErrorResult(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}