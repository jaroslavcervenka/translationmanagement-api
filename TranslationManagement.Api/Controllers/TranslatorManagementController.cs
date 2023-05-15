using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Application.Messaging.Commands;
using TranslationManagement.Application.Messaging.Commands.Translators;
using TranslationManagement.Application.Messaging.Queries;
using TranslationManagement.Application.Messaging.Queries.Translators;
using TranslationManagement.Core.Dto;
using TranslationManagement.Core.Enums;

namespace TranslationManagement.Api.Controllers
{
	[Route("api/translators")]
    public class TranslatorManagementController : BaseApiController
    {
	    private readonly ILogger<TranslatorManagementController> _logger;

	    public TranslatorManagementController(
		    IMediator mediator,
		    ILogger<TranslatorManagementController> logger) : base(mediator)
        {
	        _logger = Guard.Against.Null(logger);
        }

	    /// <summary>
	    /// Gets list of translators
	    /// </summary>
	    [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public Task<ActionResult<IEnumerable<TranslatorDto>>> GetAllTranslators(CancellationToken cancellationToken)
        {
	        var request = new GetAllTranslatorsQuery();

	        return SendAsync(request, cancellationToken);
        }

        [HttpGet("search")]
        public Task<ActionResult<IEnumerable<TranslatorDto>>> GetTranslatorsByName(
	        string name,
	        CancellationToken cancellationToken)
        {
	        var request = new GetTranslatorsByNameQuery(name);

            return SendAsync(request, cancellationToken);
        }

        [HttpPost]
        public Task<ActionResult<TranslatorDto>> AddTranslator(
	        TranslatorDto translator,
	        CancellationToken cancellationToken)
        {
	        Guard.Against.Null(translator);

	        var request = new AddTranslatorCommand(translator);

	        return SendAsync(request, cancellationToken);
        }

        [HttpPut("{id:int}/status")]
        public Task<ActionResult<TranslatorDto>> UpdateTranslatorStatus(
	        int id,
	        [FromBody]ETranslatorStatus status,
	        CancellationToken cancellationToken)
        {
            _logger.LogInformation("User status update request: {Status} for user {Id}", status, id);

            var request = new UpdateTranslatorStatusCommand(id, status);

            return SendAsync(request, cancellationToken);
        }
    }
}