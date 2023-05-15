using Ardalis.GuardClauses;
using TranslationManagement.Application.Abstractions;

namespace TranslationManagement.Application.Services;

internal class NotificationProvider : INotificationProvider
{
	private readonly INotificationChannel _channel;

	public NotificationProvider(INotificationChannel channel)
	{
		_channel = Guard.Against.Null(channel);
	}

	public ValueTask NewJobCreatedAsync(int jobId, CancellationToken cancellationToken)
	{
		Guard.Against.NegativeOrZero(jobId);

		var message = $"Job created: {jobId}";

		return _channel.WriteAsync(message, cancellationToken);
	}
}