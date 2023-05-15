using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Core.Results;

namespace TranslationManagement.Api.Controllers;

[ApiController]
public abstract class BaseApiController : Controller
{
	private readonly IMediator _mediator;

	protected BaseApiController(IMediator mediator)
	{
		_mediator = Guard.Against.Null(mediator);
	}

	protected async Task<ActionResult<T>> SendAsync<T>(
		IRequest<Result<T>> request, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var result = await _mediator.Send(request, cancellationToken);

		return CreateResponse(result);
	}

	private ActionResult<T> CreateResponse<T>(Result<T> result)
	{
		if (result.IsSuccess)
		{
			return Ok(result.Value);
		}

		if (result.HasError<NotFoundError>())
		{
			return NotFound(result.Reasons);
		}

		var statusCode = result.HasError<BadRequestError>()
			? StatusCodes.Status400BadRequest
			: StatusCodes.Status503ServiceUnavailable;

		return Problem(
			$"Error occured (result: {result.ToResult()})",
			Request.Path,
			statusCode);
	}
}