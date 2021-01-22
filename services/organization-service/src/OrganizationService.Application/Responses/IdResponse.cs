namespace OrganizationService.Application.Responses
{
    public class IdResponse
    {
        public int Id { get; private set; }
        public IdResponse(int id)
        {
            Id = id;
        }
    }
}