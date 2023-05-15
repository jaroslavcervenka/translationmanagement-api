using Ardalis.GuardClauses;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Persistence.Abstractions;

namespace TranslationManagement.Persistence;

internal sealed class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class, IEntity
{
	private readonly IAppDbContext _dbContext;

	public UnitOfWork(IAppDbContext dbContext)
	{
		_dbContext = Guard.Against.Null(dbContext);
	}

	public IRepository<TEntity> Repository()
	{
		return new GenericRepository<TEntity>(_dbContext);
	}

	public Task<int> CommitAsync(CancellationToken cancellationToken)
	{
		return _dbContext.SaveChangesAsync(cancellationToken);
	}

	public void Dispose()
	{
		_dbContext.Dispose();
	}
}