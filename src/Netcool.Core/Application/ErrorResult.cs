namespace Netcool.Core.Application
{
    public class ErrorResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public ErrorResult()
        {
        }

        public ErrorResult(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}