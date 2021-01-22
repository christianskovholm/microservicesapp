using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Infrastructure.OrganizationRepository
{
    /// <summary>
    /// Entity Framework Core implementation of IOrganizationRepository.
    /// </summary>
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly OrganizationDbContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public OrganizationRepository(OrganizationDbContext context)
        {
            _context = context;
        }

        public Organization Add(Organization organization)
        {
            return _context.Organizations.Add(organization).Entity;
        }

        public async Task<Organization> GetAsync(int id)
        {
            var organization = await _context.Organizations
                .Include(x => x.Roles)
                .Include(x => x.Departments)
                .Include(x => x.Departments).ThenInclude(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (organization == null)
                throw new DomainObjectNotFoundException($"No organization exists with id {id}.");

            return organization;
        }

        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            var organizations = await _context.Organizations
                .Include(x => x.Roles)
                .Include(x => x.Departments)
                .Include(x => x.Departments).ThenInclude(x => x.Members)
                .ToListAsync();

            return organizations;
        }

        public void Update(Organization organization)
        {
            _context.Entry(organization).State = EntityState.Modified;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Id == id);

            if (organization == null) 
                return false;

            _context.Organizations.Remove(organization);

            return true;
        }
    }
}