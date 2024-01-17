using AffiliatePMS.Domain.Affiliates;
using AffiliatePMS.Infra.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AffiliatePMS.Infra.Tests;



public class RepositoryTests
{
    private readonly Mock<DbSet<Affiliate>> mockDbSet;
    private readonly Mock<APMSDbContext> mockDbContext;
    private readonly Repository<Affiliate> repository;

    public RepositoryTests()
    {
        mockDbSet = new Mock<DbSet<Affiliate>>();
        mockDbContext = new Mock<APMSDbContext>();
        mockDbContext.Setup(db => db.Set<Affiliate>()).Returns(mockDbSet.Object);
        repository = new Repository<Affiliate>(mockDbContext.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldCallAddAsyncOnDbSet()
    {
        // Arrange
        var entity = new Affiliate();

        // Act
        await repository.AddAsync(entity);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldCallFindAsyncOnDbSet()
    {
        // Arrange
        var id = 1;

        // Act
        await repository.GetAsync(id);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.FindAsync(id), Times.Once);
    }
    [Fact]
    public void Delete_ShouldCallRemoveOnDbSet()
    {
        // Arrange
        var entity = new Affiliate();

        // Act
        repository.Delete(entity);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.Remove(entity), Times.Once);
    }


}
