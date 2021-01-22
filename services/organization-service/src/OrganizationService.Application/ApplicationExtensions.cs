using System;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrganizationService.Application.Behaviors;
using OrganizationService.Infrastructure;
using OrganizationService.Infrastructure.OrganizationRepository;

namespace OrganizationService.Application
{
    /// <summary>
    /// Contains various extension methods for use in application setup.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Retrieve a connection string from the environment variable SQL_SERVER_CONN_STR.
        /// If the env var is unset, a default connection string is returned.
        /// </summary>
        /// <returns>A Connection string.</returns>
        public static string GetSqlServerConnStr()
        {
            var connStrEnvVal = Environment.GetEnvironmentVariable("SQL_SERVER_CONN_STR");

            if (string.IsNullOrEmpty(connStrEnvVal))
            {
                return "Server=tcp:localhost,1433;Database=OrganizationsDb;User Id=sa;Password=Pass@Word;";
            }

            return connStrEnvVal;
        }

        /// <summary>
        /// Adds MediatR command handlers and pipeline behaviors to the service collection.
        /// </summary>
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));

            return services;
        }

        /// <summary>
        /// Adds the default implementation of IOrganizationRepository and an OrganizationDbContext pool to the service collection.
        /// </summary>
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connStr)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (connStr == null)
            {
                throw new ArgumentNullException(nameof(connStr));
            }

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddDbContextPool<OrganizationDbContext>(options => {
                options.UseSqlServer(connStr, sqlServerOptionsAction: sqlOptions => {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });

            return services;
        }
    }
}