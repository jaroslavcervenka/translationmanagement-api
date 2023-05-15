using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Application.Messaging.Commands.Translators;

public sealed class AddTranslatorCommand : IRequest<Result<TranslatorDto>>
{
	public TranslatorDto Payload { get; }

	public AddTranslatorCommand(TranslatorDto data)
	{
		Payload = Guard.Against.Null(data);
	}
}

public sealed class AddTranslatorCommandHandler
	: IRequestHandler<AddTranslatorCommand, Result<TranslatorDto>>
{
	private readonly IUnitOfWork<Translator> _uow;
	private readonly IMapper _mapper;

	public AddTranslatorCommandHandler(IUnitOfWork<Translator> uow, IMapper mapper)
	{
		_uow = Guard.Against.Null(uow);
		_mapper = Guard.Against.Null(mapper);
	}

	public async Task<Result<TranslatorDto>> Handle(AddTranslatorCommand request, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var translator = _mapper.Map<Translator>(request.Payload);
		var repositoryResult = await _uow.Repository().AddAsync(translator, cancellationToken);

		if (repositoryResult.IsFailed)
		{
			return repositoryResult.ToResult();
		}

		return Result.Ok(_mapper.Map<TranslatorDto>(repositoryResult.Value));
	}
}