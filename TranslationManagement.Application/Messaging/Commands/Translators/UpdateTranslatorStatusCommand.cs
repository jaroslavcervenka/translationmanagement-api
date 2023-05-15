using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;
using TranslationManagement.Core.Enums;

namespace TranslationManagement.Application.Messaging.Commands.Translators;

public sealed class UpdateTranslatorStatusCommand : IRequest<Result<TranslatorDto>>
{
	public int TranslatorId { get; }
	public ETranslatorStatus Status { get; }

	public UpdateTranslatorStatusCommand(int translatorId , ETranslatorStatus status)
	{
		TranslatorId = Guard.Against.NegativeOrZero(translatorId);
		Status = Guard.Against.EnumOutOfRange(status);
	}
}

public sealed class UpdateTranslatorStatusCommandHandler
	: IRequestHandler<UpdateTranslatorStatusCommand, Result<TranslatorDto>>
{
	private readonly IUnitOfWork<Translator> _uow;
	private readonly IMapper _mapper;

	public UpdateTranslatorStatusCommandHandler(IUnitOfWork<Translator> uow, IMapper mapper)
	{
		_uow = Guard.Against.Null(uow);
		_mapper = Guard.Against.Null(mapper);
	}

	public async Task<Result<TranslatorDto>> Handle(UpdateTranslatorStatusCommand request, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var repository = _uow.Repository();
		var getTranslatorResult = await repository.GetAsync(request.TranslatorId, cancellationToken);

		if (getTranslatorResult.IsFailed)
		{
			return getTranslatorResult.ToResult();
		}

		var translator = getTranslatorResult.Value;
		translator.SetStatus(request.Status);
		await _uow.CommitAsync(cancellationToken);

		return Result.Ok(_mapper.Map<TranslatorDto>(translator));
	}
}