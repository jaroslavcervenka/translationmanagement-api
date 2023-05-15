using Ardalis.GuardClauses;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Domain;

namespace TranslationManagement.Application.Messaging.Queries.Translators;

public sealed class GetTranslatorQuery : IRequest<Result<Translator>>
{
	public int TranslatorId { get; }

	public GetTranslatorQuery(int translatorId)
	{
		TranslatorId = Guard.Against.NegativeOrZero(translatorId);
	}
}

public sealed class GetTranslatorQueryHandler
	: IRequestHandler<GetTranslatorQuery, Result<Translator>>
{
	private readonly IUnitOfWork<Translator> _uow;

	public GetTranslatorQueryHandler(IUnitOfWork<Translator> uow)
	{
		_uow = Guard.Against.Null(uow);
	}

	public Task<Result<Translator>> Handle(GetTranslatorQuery request, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		return _uow.Repository().GetAsync(request.TranslatorId, cancellationToken);
	}
}