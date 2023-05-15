namespace TranslationManagement.Application.Abstractions;

public interface INotificationSender
{
	Task RunAsync(CancellationToken cancellationToken);
}