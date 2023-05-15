using Ardalis.GuardClauses;
using FluentResults;

namespace TranslationManagement.Core.Results;

public class ValidationError : Error
{
	public IReadOnlyDictionary<string, string[]> Failures { get; }

	public ValidationError(IReadOnlyDictionary<string, string[]> failures)
	{
		Failures = Guard.Against.Null(failures);
	}
}