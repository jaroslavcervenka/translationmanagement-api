using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using TranslationManagement.Abstractions.Infrastructure;
using TranslationManagement.Application.Abstractions;

namespace TranslationManagement.Application.Services;

internal sealed class NotificationSender : INotificationSender
{
	private readonly INotificationChannel _channel;
	private readonly INotificationClient _notificationClient;
	private readonly ILogger<NotificationSender> _logger;

	public NotificationSender(
		INotificationChannel channel,
		INotificationClient notificationClient,
		ILogger<NotificationSender> logger)
	{
		_channel = Guard.Against.Null(channel);
		_notificationClient = Guard.Against.Null(notificationClient);
		_logger = Guard.Against.Null(logger);
	}

	public async Task RunAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var message = await _channel.ReadAsync(cancellationToken);
			await SendAsync(message, cancellationToken);
		}
	}

	private async Task SendAsync(string message, CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var sendResult = await _notificationClient.SendAsync(message, cancellationToken);

			if (sendResult.IsSuccess)
			{
				_logger.LogInformation("New job notification sent");
				return;
			}
		}
	}
}