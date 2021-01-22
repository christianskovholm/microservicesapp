namespace OrganizationService.Application.Responses
{
    public class HttpResponseBody
    {
        public object Data { get; private set; }
        public HttpResponseBody(object data)
        {
            Data = data;
        }
    }
}