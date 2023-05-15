using System;
using System.IO;
using System.Reflection;
using Ardalis.GuardClauses;
using BetterHostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TranslationManagement.Api.Workers;

namespace TranslationManagement.Api.Extensions;

public static class ApiServiceCollectionExtensions
{
	public static IServiceCollection AddWorkers(this IServiceCollection services)
	{
		Guard.Against.Null(services);

		services
			.AddBetterHostedServices()
			.AddHostedService<NotificationWorker>();

		return services;
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		Guard.Against.Null(services);

		services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslationManagement.Api", Version = "v1" });
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

		return services;
	}

	public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
	{
		services
			.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						var headers = new[] { "Content-Type", "Origin", "Accept", "Authorization", "Content-Length", "X-Requested-With", "x-vektra-trace-id" };
						builder.WithOrigins("http://localhost:3000")
							.AllowAnyHeader()
							.AllowAnyMethod()
							.AllowCredentials();
					});
			});

		return services;
	}
}