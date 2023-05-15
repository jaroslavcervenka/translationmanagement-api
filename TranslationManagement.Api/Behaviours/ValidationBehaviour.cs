using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentValidation;
using MediatR;
using TranslationManagement.Api.Services;

namespace TranslationManagement.Api.Behaviours;

public class ValidationBehaviour
	<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	where TResponse : class
{
	private readonly RequestValidationService<TRequest> _validation;

	public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
	{
		_validation = new RequestValidationService<TRequest>(validators);
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);
		Guard.Against.Null(next);

		var validationResult = _validation.Validate(request);

		if (validationResult.IsSuccess)
		{
			return await next();
		}

		if (validationResult is TResponse response)
		{
			return response;
		}

		throw new InvalidOperationException("Unsupported request type");
	}
}