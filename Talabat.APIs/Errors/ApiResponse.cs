namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefultMessageForStatusCode(StatusCode);
        }

        private string? GetDefultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400=>"BadRequest",
                401 => "You Are Not Authourized",
                404 =>"Resours Not Found",
                500=> "Internal Server Error",
                _=> null
            };
            
        }
    }
}
