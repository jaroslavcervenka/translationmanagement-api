using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Application.Messaging.Commands;
using TranslationManagement.Application.Messaging.Commands.Jobs;
using TranslationManagement.Application.Messaging.Queries;
using TranslationManagement.Application.Messaging.Queries.Jobs;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Api.Controllers
{
	[Route("api/jobs")]
    public class TranslationJobController : BaseApiController
    {
	    private readonly ILogger<TranslatorManagementController> _logger;

	    public TranslationJobController(
	        IMediator mediator,
	        ILogger<TranslatorManagementController> logger) : base(mediator)
        {
	        _logger = logger;
        }

	    /// <summary>
	    /// Gets list of translation jobs
	    /// </summary>
	    [HttpGet]
	    [ProducesResponseType(StatusCodes.Status200OK)]
	    [ProducesResponseType(StatusCodes.Status400BadRequest)]
	    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public Task<ActionResult<IEnumerable<TranslationJobDto>>> GetJobs(CancellationToken cancellationToken)
        {
	        var request = new GetAllTranslationJobsQuery();

	        return SendAsync(request, cancellationToken);
        }

        [HttpPost]
        public Task<ActionResult<TranslationJobDto>> CreateJob(AddTranslationJobDto job, CancellationToken cancellationToken)
        {
	        var request = new CreateJobCommand(job);

	        return SendAsync(request, cancellationToken);
        }

        [HttpPost("with-file")]
        public Task<ActionResult<TranslationJobDto>> CreateJobWithFile(
	        IFormFile file,
	        string customer,
	        CancellationToken cancellationToken)
        {
	        Guard.Against.Null(file);
	        Guard.Against.NullOrEmpty(customer);

	        using var stream = file.OpenReadStream();
	        var request = new CreateJobWithFileCommand(file.FileName, stream, customer);

	        return SendAsync(request, cancellationToken);
        }

        [HttpPut("{id:int}/status")]
        public Task<ActionResult<TranslationJobDto>> UpdateJobStatus(
	        int id,
	        [FromBody]UpdateTranslationJobStatusDto payload,
	        CancellationToken cancellationToken)
        {
            _logger.LogInformation(
	            "Job status update request received: {status} for job {id} by translator {translatorId}",
	            payload.Status,
	            id,
	            payload.TranslatorId);

            var request = new UpdateJobStatusCommand(id, payload.TranslatorId, payload.Status);

            return SendAsync(request, cancellationToken);
        }
    }
}