namespace Talabat.APIs.Errors
{
    public class ApiExcptionRespons : ApiResponse
    {
        public string? Detailes { get; set; }
        public ApiExcptionRespons(int statusCode, string? message = null, string? detailes = null) : base(statusCode, message)
        {
            Detailes = detailes;
        }
    }
}
