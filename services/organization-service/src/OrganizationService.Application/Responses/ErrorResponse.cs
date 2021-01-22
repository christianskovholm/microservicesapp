namespace OrganizationService.Application.Responses
{
    public class ErrorResponse
    {
        public int Code { get; private set; }
        public object Message { get; private set; }

        public ErrorResponse(int code, object message)
        {
            Code = code;
            Message = message;
        }
    }
}