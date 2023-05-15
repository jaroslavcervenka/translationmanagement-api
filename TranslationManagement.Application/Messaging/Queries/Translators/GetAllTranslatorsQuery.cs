using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Application.Messaging.Queries.Translators;

public sealed class GetAllTranslatorsQuery : IRequest<Result<IEnumerable<TranslatorDto>>>
{

}

public sealed class GetAllTranslatorsQueryHandler
	: IRequestHandler<GetAllTranslatorsQuery, Result<IEnumerable<TranslatorDto>>>
{
	private readonly IUnitOfWork<Translator> _uow;
	private readonly IMapper _mapper;

	public GetAllTranslatorsQueryHandler(IUnitOfWork<Translator> uow, IMapper mapper)
	{
		_uow = Guard.Against.Null(uow);
		_mapper = Guard.Against.Null(mapper);
	}

	public async Task<Result<IEnumerable<TranslatorDto>>> Handle(
		GetAllTranslatorsQuery request,
		CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var repositoryResult = await _uow.Repository().FindAllAsync(cancellationToken);

		if (repositoryResult.IsFailed)
		{
			return repositoryResult.ToResult();
		}

		var dto = _mapper.Map<IEnumerable<TranslatorDto>>(repositoryResult.Value);

		return Result.Ok(dto);
	}
}