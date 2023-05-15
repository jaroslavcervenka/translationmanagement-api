namespace TranslationManagement.Application.Abstractions;

public interface INotificationProvider
{
	ValueTask NewJobCreatedAsync(int jobId, CancellationToken cancellationToken);
}