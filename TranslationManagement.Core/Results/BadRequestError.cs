using FluentResults;

namespace TranslationManagement.Core.Results;

public class BadRequestError : Error
{
	public BadRequestError(string message) : base(message)
	{

	}
}