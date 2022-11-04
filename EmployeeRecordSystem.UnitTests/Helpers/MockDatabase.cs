using Duende.IdentityServer.EntityFramework.Options;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace EmployeeRecordSystem.UnitTests.Helpers;

public abstract class MockDatabase
{
	protected Mock<ApplicationDbContext> DbContextMock { get; }

	protected static Group GroupMock { get; } = new()
	{
		Id = Guid.NewGuid()
	};
	
	protected static Employee EmployeeMock { get; } = new()
	{
		Id = Guid.NewGuid()
	};
	
	protected static WithdrawalRequest WithdrawalRequestMock { get; } = new()
	{
		Id = Guid.NewGuid(),
		EmployeeId = Guid.NewGuid()
	};
	
	protected MockDatabase()
	{
		var dbContextMock = InitDbContextMock();
		ConfigureDbSets(dbContextMock);
		DbContextMock =  dbContextMock;
	}

	private static void ConfigureDbSets(Mock<ApplicationDbContext> dbContextMock)
	{
		var groupDbSetMock = CreateDbSetMock(GroupMock);
		dbContextMock.Setup(m => m.Groups).Returns(groupDbSetMock.Object);

		var employeeDbSetMock = CreateDbSetMock(EmployeeMock);
		dbContextMock.Setup(m => m.Users).Returns(employeeDbSetMock.Object);

		var withdrawalRequestDbSetMock = CreateDbSetMock(WithdrawalRequestMock);
		dbContextMock.Setup(m => m.WithdrawalRequests).Returns(withdrawalRequestDbSetMock.Object);
	}

	private static Mock<ApplicationDbContext> InitDbContextMock()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		var options = Options.Create(new OperationalStoreOptions());
		var dbContextMock = new Mock<ApplicationDbContext>(builder.Options, options);
		return dbContextMock;
	}

	private static Mock<DbSet<TEntity>> CreateDbSetMock<TEntity>(TEntity data) where TEntity : class
	{
		var queryable = new List<TEntity>
		{
			data
		}.AsQueryable();
		
		var dbSetMock = new Mock<DbSet<TEntity>>();

		dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
		dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
		dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
		dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);
		
		return dbSetMock;
	}
}
