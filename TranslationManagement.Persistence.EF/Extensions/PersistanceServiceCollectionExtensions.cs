using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Domain;
using TranslationManagement.Persistence.Abstractions;

namespace TranslationManagement.Persistence.Extensions;

public static class PersistenceServiceCollectionExtensions
{
	private const string ConnectionString = "TranslationAppDatabase";

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		Guard.Against.Null(services);
		Guard.Against.Null(configuration);
		var connString = Guard.Against.NullOrEmpty(configuration.GetConnectionString(ConnectionString));

		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlite(connString));

		services
			.AddScoped<IAppDbContext, AppDbContext>()
			.AddScoped<IUnitOfWork<Translator>, UnitOfWork<Translator>>()
			.AddScoped<IUnitOfWork<TranslationJob>, UnitOfWork<TranslationJob>>();

		return services;
	}
}