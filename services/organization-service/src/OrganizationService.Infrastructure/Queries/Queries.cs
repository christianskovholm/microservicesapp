using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Domain.Exceptions;

namespace OrganizationService.Infrastructure.Queries
{
    /// <summary>
    /// Implementation of IQueries using dapper.
    /// </summary>
    public class Queries : IQueries, IDisposable
    {
        private readonly SqlConnection _conn;

        public Queries(string connStr)
        {
            _conn = new SqlConnection(connStr);
            _conn.Open();
        }

        public void Dispose()
        {
            _conn.Dispose();
        }

        public async Task<JObject> GetOrganizationAsync(int id)
        {
            var sql = @"SELECT TOP 1
                            o.Id as id, o.Name as name, o.Description as description, o.Created as created, o.LastUpdated as lastUpdated,
                            (
                                SELECT d.Id as id, d.Name as name, d.Created as created, d.LastUpdated as lastUpdated,
                                (
                                    SELECT m.Id as id, m.Name as name, m.Created as created, m.LastUpdated as lastUpdated, r.Id as roleId
                                    FROM dbo.members m
                                    LEFT JOIN dbo.roles r ON r.Id = m.RoleId
                                    WHERE m.DepartmentId = d.Id FOR JSON PATH
                                ) members
                                FROM dbo.departments d WHERE d.OrganizationId = o.Id FOR JSON AUTO
                            ) departments,
                            (
                                SELECT r.Id as id, r.Name as name, r.Created as created, r.LastUpdated as lastUpdated 
                                FROM dbo.roles r WHERE r.OrganizationId = o.Id FOR JSON AUTO
                            ) roles
                        FROM dbo.organizations o
                        WHERE o.Id = @id
                        FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER";

            var result = await _conn.QuerySingleOrDefaultAsync<string>(sql, new { id });

            if (result == null)
                throw new DomainObjectNotFoundException($"No organization exists with id {id}.");

            return JObject.Parse(result);
        }
    }
}