using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Application.Messaging.Queries.Jobs;

public sealed class GetAllTranslationJobsQuery : IRequest<Result<IEnumerable<TranslationJobDto>>>
{

}

public sealed class GetAllTranslationJobsQueryHandler
	: IRequestHandler<GetAllTranslationJobsQuery, Result<IEnumerable<TranslationJobDto>>>
{
	private readonly IUnitOfWork<TranslationJob> _uow;
	private readonly IMapper _mapper;

	public GetAllTranslationJobsQueryHandler(IUnitOfWork<TranslationJob> uow, IMapper mapper)
	{
		_uow = Guard.Against.Null(uow);
		_mapper = Guard.Against.Null(mapper);
	}

	public async Task<Result<IEnumerable<TranslationJobDto>>> Handle(GetAllTranslationJobsQuery request, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var repositoryResult = await _uow.Repository().FindAllAsync(cancellationToken);

		if (repositoryResult.IsFailed)
		{
			return repositoryResult.ToResult();
		}

		var dto = _mapper.Map<IEnumerable<TranslationJobDto>>(repositoryResult.Value);

		return Result.Ok(dto);
	}
}