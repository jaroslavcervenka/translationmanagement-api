using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using FluentResults;
using FluentValidation;
using TranslationManagement.Core.Results;

namespace TranslationManagement.Api.Services;

internal sealed class RequestValidationService<TRequest>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public RequestValidationService(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = Guard.Against.Null(validators);
	}

	public Result Validate(TRequest request)
	{
		Guard.Against.Null(request);

		if (!_validators.Any())
		{
			return Result.Ok();
		}

		var context = new ValidationContext<TRequest>(request);
		var validationResults =
			_validators.Select(v => v.Validate(context));
		var failures = validationResults.SelectMany(r => r.Errors)
			.Where(f => f != null)
			.GroupBy(x => x.PropertyName,
				x => x.ErrorMessage,
				(propertyName, errorMessages) => new
				{
					Key = propertyName,
					Values = errorMessages.Distinct().ToArray()
				})
			.ToDictionary(x => x.Key, x => x.Values);

		return failures.Any() ? Result.Fail(new ValidationError(failures)) : Result.Ok();
	}
}