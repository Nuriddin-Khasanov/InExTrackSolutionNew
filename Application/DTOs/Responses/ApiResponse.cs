namespace Application.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int _code, T? data, string message)
        {
            Code = _code;
            Message = message;
            Data = data;
        }
        public ApiResponse(int _code, string errorMessage)
        {
            Code = _code;
            Message = errorMessage;
        }
    }
}
