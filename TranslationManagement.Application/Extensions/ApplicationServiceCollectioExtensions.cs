using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Application.Abstractions;
using TranslationManagement.Application.Services;

namespace TranslationManagement.Application.Extensions;

public static class ApplicationServiceCollectioExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		Guard.Against.Null(services);

		services
			.AddSingleton<INotificationProvider, NotificationProvider>()
			.AddTransient<INotificationSender,NotificationSender>()
			.AddSingleton<INotificationChannel,NotificationChannel>()
			.AddSingleton<INotificationProvider,NotificationProvider>()
			.AddSingleton<ITranslationPriceCalculator,TranslationPriceCalculator>();

		return services;
	}
}