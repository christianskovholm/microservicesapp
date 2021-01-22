using Xunit;
using OrganizationService.Infrastructure.OrganizationRepository;
using OrganizationService.Infrastructure;
using OrganizationService.Domain.Aggregates.Organization;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System;
using OrganizationService.Application.Factories;

namespace OrganizationService.IntegrationTests
{
    public class OrganizationRepositoryIntegrationTests : IDisposable
    {
        private readonly IOrganizationRepository _repository;
        private readonly OrganizationDbContext _context;

        public OrganizationRepositoryIntegrationTests()
        {
            _context = new OrganizationDbContextFactory().CreateDbContext(null);
            _repository = new OrganizationRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async void Add_Should_Add_Organization_To_Repository()
        {
            // Arrange
            var organization = new Organization("test", "test");

            // Act
            _repository.Add(organization);
            await _repository.UnitOfWork.SaveChangesAsync();

            // Assert
            var retrievedOrganization = await _context.Organizations.SingleAsync(x => x.Id == organization.Id);
            retrievedOrganization.Should().BeEquivalentTo(organization);
        }

        [Fact]
        public async void GetAsync_Should_Get_Organization()
        {
            // Arrange
            var organization = new Organization("test", "test");
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            // Act
            var retrievedOrganization = await _repository.GetAsync(organization.Id);

            // Assert
            retrievedOrganization.Should().BeEquivalentTo(organization);
        }

        [Fact]
        public async void Update_Should_Update_Organization()
        {
            // Arrange
            var organization = new Organization("test", "test");
            
            _context.Add(organization);
            await _context.SaveChangesAsync();
            organization.CreateDepartment("sales");

            // Act
            _repository.Update(organization);
            await _repository.UnitOfWork.SaveChangesAsync();

            // Assert
            var retrievedOrganization = await _context.Organizations.FirstOrDefaultAsync(x => x.Id == organization.Id);
            retrievedOrganization.Should().BeEquivalentTo(organization);
        }

        [Fact]
        public async void Delete_Should_Delete_Organization()
        {
            // Arrange
            var organization = new Organization("test", "test");
            _context.Add(organization);
            await _context.SaveChangesAsync();
            var id = organization.Id;

            // Act
            await _repository.DeleteAsync(id);
            await _repository.UnitOfWork.SaveChangesAsync();

            // Assert
            var retrievedOrganization = await _context.Organizations.FirstOrDefaultAsync(x => x.Id == id);
            retrievedOrganization.Should().BeNull();
        }
    }
}