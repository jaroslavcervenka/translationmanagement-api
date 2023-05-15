namespace TranslationManagement.Abstractions.Persistance;

public interface IUnitOfWork<TEntity> : IDisposable where TEntity : class, IEntity
{
	IRepository<TEntity> Repository();

	Task<int> CommitAsync(CancellationToken cancellationToken);
}