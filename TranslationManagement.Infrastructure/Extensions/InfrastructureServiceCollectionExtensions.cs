using Ardalis.GuardClauses;
using External.ThirdParty.Services;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Abstractions.Infrastructure;
using TranslationManagement.Infrastructure.Abstractions;
using TranslationManagement.Infrastructure.Clients;
using TranslationManagement.Infrastructure.Policies;

namespace TranslationManagement.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		Guard.Against.Null(services);

		services
			.AddTransient<INotificationClient, NotificationClient>()
			.AddTransient<INotificationService, UnreliableNotificationService>()
			.AddSingleton<IClientPolicy, ClientPolicy>();

		return services;
	}
}