using FluentResults;

namespace TranslationManagement.Abstractions.Infrastructure;

public interface INotificationClient
{
	Task<Result> SendAsync(string message, CancellationToken cancellationToken);
}