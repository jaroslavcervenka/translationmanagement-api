using FluentResults;
using TranslationManagement.Core.Domain;

namespace TranslationManagement.Infrastructure.Abstractions;

public interface ITranslationJobRepository
{
	Task<IResult<IReadOnlyCollection<TranslationJob>>> GetAllAsync(CancellationToken cancellationToken);

	Task<IResult<TranslationJob>> GetAsync(int id, CancellationToken cancellationToken);

	Task<IResult<TranslationJob>> AddAsync(TranslationJob job, CancellationToken cancellationToken);

	Task<IResult<TranslationJob>> UpdateAsync(TranslationJob job, CancellationToken cancellationToken);
}