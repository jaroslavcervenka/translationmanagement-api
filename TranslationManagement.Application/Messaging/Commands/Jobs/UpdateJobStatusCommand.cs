using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Application.Messaging.Queries.Translators;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;
using TranslationManagement.Core.Enums;
using TranslationManagement.Core.Results;

namespace TranslationManagement.Application.Messaging.Commands.Jobs;

public sealed class UpdateJobStatusCommand : IRequest<Result<TranslationJobDto>>
{
	public int TranslationJobId { get; }

	public int TranslatorId { get; }

	public ETranslationJobStatus Status { get; }

	public UpdateJobStatusCommand(int translationJobId, int translatorId, ETranslationJobStatus status)
	{
		TranslationJobId = Guard.Against.NegativeOrZero(translationJobId);
		TranslatorId = Guard.Against.NegativeOrZero(translatorId);
		Status = Guard.Against.EnumOutOfRange(status);
	}
}

public sealed class UpdateJobStatusCommandHandler
	: IRequestHandler<UpdateJobStatusCommand, Result<TranslationJobDto>>
{
	private readonly IUnitOfWork<TranslationJob> _uowJob;
	private readonly IRequestHandler<GetTranslatorQuery, Result<Translator>> _getTranslatorHandler;
	private readonly IMapper _mapper;

	public UpdateJobStatusCommandHandler(
		IUnitOfWork<TranslationJob> uowJob,
		IRequestHandler<GetTranslatorQuery, Result<Translator>> getTranslatorHandler,
		IMapper mapper)
	{
		_uowJob = Guard.Against.Null(uowJob);
		_getTranslatorHandler = Guard.Against.Null(getTranslatorHandler);
		_mapper = Guard.Against.Null(mapper);
	}

	public async Task<Result<TranslationJobDto>> Handle(
		UpdateJobStatusCommand request,
		CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var isCertifiedResult = await IsTranslatorCertifiedAsync(request.TranslatorId, cancellationToken);

		if (isCertifiedResult.IsFailed)
		{
			return isCertifiedResult.ToResult<TranslationJobDto>();
		}

		var jobRepository = _uowJob.Repository();
		var getJobResult = await jobRepository.GetAsync(request.TranslationJobId, cancellationToken);

		if (getJobResult.IsFailed)
		{
			return getJobResult.ToResult();
		}

		var job = getJobResult.Value;
		var updateStatusResult = job.UpdateStatus(request.Status);

		if (updateStatusResult.IsFailed)
		{
			var error = new BadRequestError("Change job status is not allowed");
			error.Reasons.AddRange(updateStatusResult.Errors);
			return Result.Fail(error);
		}

		await _uowJob.CommitAsync(cancellationToken);

		return Result.Ok(_mapper.Map<TranslationJobDto>(job));
	}

	private async Task<Result> IsTranslatorCertifiedAsync(int translatorId, CancellationToken cancellationToken)
	{
		var getTranslatorResult = await _getTranslatorHandler.Handle(new GetTranslatorQuery(translatorId), cancellationToken);

		if (getTranslatorResult.IsFailed)
		{
			return getTranslatorResult.ToResult();
		}

		if (getTranslatorResult.Value.Status != ETranslatorStatus.Certified)
		{
			return Result.Fail(new BadRequestError($"Translator '{translatorId}' is not certified."));
		}

		return Result.Ok();
	}
}