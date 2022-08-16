namespace ItAccept.Teste.Domain.Models
{
    public class ApiResponse
    {
        public ApiResponseState State { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }
        public Object Erros {get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(ApiResponseState state, string message, Object data)
        {
            State = state;
            Message = message;
            Data = state == ApiResponseState.Success ? data : null;
            Erros = state == ApiResponseState.Failed ? data : null;
        }

        
        public ApiResponse(ApiResponseState state, string message)
        {
            State = state;
            Message = message;
        }
    }
}
