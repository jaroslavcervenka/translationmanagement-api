using System.Linq.Expressions;
using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Results;
using TranslationManagement.Persistence.Abstractions;

namespace TranslationManagement.Persistence;

internal sealed class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
	private readonly IAppDbContext _dbContext;

	public GenericRepository(IAppDbContext dbContext)
	{
		_dbContext = Guard.Against.Null(dbContext);
	}

	public async Task<Result<TEntity>> GetAsync(int id, CancellationToken cancellationToken)
	{
		Guard.Against.Null(id);

		var entity = await _dbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

		return entity != null ? Result.Ok(entity) : Result.Fail(new NotFoundError());
	}

	public async Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken)
	{
		var entities = await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);

		return Result.Ok<IReadOnlyCollection<TEntity>>(entities);
	}

	public async Task<Result<IReadOnlyCollection<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
	{
		Guard.Against.Null(predicate);

		var entities = await _dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);

		return Result.Ok<IReadOnlyCollection<TEntity>>(entities);
	}

	public async Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken)
	{
		Guard.Against.Null(entity);

		var entry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

		return Result.Ok(entry.Entity);
	}

	public Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
	{
		Guard.Against.Null(entity);

		_dbContext.Entry(entity).State = EntityState.Modified;

		return Task.FromResult(Result.Ok(entity));
	}

	public Task<Result<TEntity>> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
	{
		Guard.Against.Null(entity);

		_dbContext.Set<TEntity>().Remove(entity);

		return Task.FromResult(Result.Ok(entity));
	}
}