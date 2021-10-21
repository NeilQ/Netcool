namespace Netcool.Core.AspNetCore
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
