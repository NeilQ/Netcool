namespace Netcool.Core.AspNetCore
{
    public class ValidateResult
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public ValidateResult()
        {
        }

        public ValidateResult(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}