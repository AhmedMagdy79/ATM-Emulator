namespace ATM__Emulator.Dtos
{
    public class Response<T>
    {
        private readonly string Success = "Success";

        private readonly string Faild = "Faild";

        public string? ErrorMessage { get; private set; }

        public string ResponseStatus { get; private set; }

        public int StatusCode { get; private set; }

        public T Result { get; private set; }

        private Response()
        {
            
        }

        private Response(string ErrorMessage, T Result, int StatusCode)
        {
            this.ErrorMessage = ErrorMessage;
            this.Result = Result;
            this.ResponseStatus = Faild;
            this.StatusCode = StatusCode;
        }

        private Response(T Result, int StatusCode)
        {
            this.ErrorMessage = String.Empty;
            this.Result = Result;
            this.ResponseStatus = Success;
            this.StatusCode = StatusCode;
        }

        public static Response<T> CreateErrorResponse(string ErrorMessage, T Result, int StatusCode )
        {
            return new Response<T>(ErrorMessage, Result, StatusCode);
        }

        public static Response<T> CreateSuccessResponse( T Result, int StatusCode)
        {
            return new Response<T>(Result, StatusCode);
        }
    }
}
