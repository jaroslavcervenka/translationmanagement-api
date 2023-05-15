using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TranslationManagement.Persistence.Abstractions;

internal interface IAppDbContext : IDisposable
{
	EntityEntry Entry(object entity);

	DbSet<IEntity> Set<IEntity>() where IEntity : class;

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}