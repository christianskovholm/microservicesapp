using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Application.Factories;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.DomainEvents;
using OrganizationService.Domain.SeedWork;
using OrganizationService.Infrastructure;

namespace OrganizationService.Application.Behaviors
{
    /// <summary>
    /// Pipeline behavior for command database transactions.
    /// </summary>
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly OrganizationDbContext _context;

        public TransactionPipelineBehavior(OrganizationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executes the specified command, and persists all domain events within the updated aggregate root of the command.
        /// </summary>
        /// <returns>Task object containing the result of the command.</returns>
        public async Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var response = await next();
                    var domainEvents = new List<DomainEvent>();
                    var entries =  _context.ChangeTracker.Entries<DomainObject>();
                    var organization = entries.Single(x => x.Entity is Organization);

                    if (organization.State == EntityState.Added)
                    {
                        var organizationCreatedDomainEvent = new OrganizationCreatedDomainEvent(organization.Entity as Organization);
                        domainEvents.Add(organizationCreatedDomainEvent); 
                    }

                    domainEvents.AddRange(entries.SelectMany(x => x.Entity.GetDomainEvents().OrderBy(x => x.Timestamp)));
                    await _context.SaveChangesAsync(cancellationToken);

                    foreach (var d in domainEvents)
                    {
                        if (d.GetType().BaseType == typeof(CreatedDomainEvent))
                        {
                            d.DomainObject.Created = d.Timestamp;
                        }

                        d.DomainObject.LastUpdated = d.Timestamp;
                        var e = EventFactory.Create(d, organization.Entity.Id);

                        _context.Events.Add(e);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync();

                    return response;
                }
            });
        }
    }
}