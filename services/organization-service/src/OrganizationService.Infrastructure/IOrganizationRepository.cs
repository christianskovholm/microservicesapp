using System.Collections.Generic;
using System.Threading.Tasks;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Infrastructure.OrganizationRepository
{
    /// <summary>
    /// Repository for creating, retrieving, updating and deleting organizations.
    /// </summary>
    public interface IOrganizationRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add a new organization.
        /// </summary>
        Organization Add(Organization organization);

        /// <summary>
        /// Get the specified organization.
        /// </summary>
        Task<Organization> GetAsync(int id);

        /// <summary>
        /// Get all organizations.
        /// </summary>
        Task<IEnumerable<Organization>> GetAllAsync();

        /// <summary>
        /// Update the specified organization.
        /// </summary>
        void Update(Organization organization);

        /// <summary>
        /// Delete the specified organization.
        /// </summary>
        /// <returns>1 if the deletion succeeded, 0 if it didn't.</returns>
        Task<bool> DeleteAsync(int id);
    }
}