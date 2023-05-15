using System.Linq.Expressions;
using FluentResults;

namespace TranslationManagement.Abstractions.Persistance;

public interface IRepository<TEntity> where TEntity: class, IEntity
{
	Task<Result<TEntity>> GetAsync(int id, CancellationToken cancellationToken);

	Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken);

	Task<Result<IReadOnlyCollection<TEntity>>> FindAsync(
		Expression<Func<TEntity, bool>> predicate,
		CancellationToken cancellationToken);

	Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken);

	Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

	Task<Result<TEntity>> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}