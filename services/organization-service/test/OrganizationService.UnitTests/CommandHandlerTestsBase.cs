using Moq;
using OrganizationService.Infrastructure.OrganizationRepository;

namespace OrganizationService.UnitTests
{
    public abstract class CommandHandlerTestBase
    {
        protected Mock<IOrganizationRepository> OrganizationRepositoryMock { get; private set; }
    
        protected CommandHandlerTestBase()
        {
            OrganizationRepositoryMock = new Mock<IOrganizationRepository>();
        }
    }
}