namespace API.Errors
{
    public class ApiResponse
    { 
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request",
                401 => "Not authorized",
                404 => "Not found",
                500 => "Server error",
                _ => "Error"
            };
        }
    }
}