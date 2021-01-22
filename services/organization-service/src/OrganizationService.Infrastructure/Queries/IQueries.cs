using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OrganizationService.Infrastructure.Queries
{
    /// <summary>
    /// Read implementation of the CQRS pattern. Contains methods for querying organizations. 
    /// </summary>
    public interface IQueries
    {
        /// <summary>
        /// Retrieve organization with the specified id and all of its child domain objects.
        /// </summary>
        /// <returns>JObject representation of organization.</returns>
        Task<JObject> GetOrganizationAsync(int id);
    }
}