using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Api.Behaviours;
using TranslationManagement.Api.Converters;
using TranslationManagement.Api.Extensions;
using TranslationManagement.Api.Middlewares;
using TranslationManagement.Api.Validators;
using TranslationManagement.Application.Extensions;
using TranslationManagement.Application.Mapping;
using TranslationManagement.Application.Messaging.Queries;
using TranslationManagement.Application.Messaging.Queries.Jobs;
using TranslationManagement.Core.Dto;
using TranslationManagement.Infrastructure.Extensions;
using TranslationManagement.Persistence;
using TranslationManagement.Persistence.Extensions;

namespace TranslationManagement.Api;


public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		var services = builder.Services;
		var configuration = builder.Configuration;

		services
			.AddWorkers()
			.AddApplication()
			.AddInfrastructure()
			.AddPersistence(configuration);

		services
			.AddControllers()
			.AddJsonOptions(c =>
			{
				c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new UpperCaseNamingPolicy()));
			});
		services
			.AddSwagger()
			.AddCorsPolicy()
			.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<GetAllTranslationJobsQuery>())
			.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
			.AddAutoMapper(typeof(AppMappingProfile))
			.AddValidatorsFromAssemblyContaining<CreateJobCommandValidator>();

		var app = builder.Build();
		app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
		app
			.UseSwagger()
			.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationManagement.Api v1"))
			.UseCors()
			.UseRouting()
			.UseAuthorization();
		app.MapControllers();

		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			db.Database.Migrate();
		}

		app.Run();
	}
}