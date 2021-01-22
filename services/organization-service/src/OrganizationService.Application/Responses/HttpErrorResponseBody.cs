namespace OrganizationService.Application.Responses
{
    public class HttpErrorResponse
    {
        public ErrorResponse Error { get; private set; }

        public HttpErrorResponse(int code, object message)
        {
            Error = new ErrorResponse(code, message);
        }
    }
}