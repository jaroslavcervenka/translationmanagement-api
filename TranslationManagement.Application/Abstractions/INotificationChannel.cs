namespace TranslationManagement.Application.Abstractions;

public interface INotificationChannel
{
	ValueTask<string> ReadAsync(CancellationToken cancellationToken);

	ValueTask WriteAsync(string message, CancellationToken cancellationToken);
}