using System.Threading.Channels;
using Ardalis.GuardClauses;
using TranslationManagement.Application.Abstractions;

namespace TranslationManagement.Application.Services;

internal class NotificationChannel : INotificationChannel
{
	private readonly Channel<string> _channel;

	public NotificationChannel()
	{
		_channel = Channel.CreateUnbounded<string>();
	}

	public ValueTask<string> ReadAsync(CancellationToken cancellationToken)
	{
		return _channel.Reader.ReadAsync(cancellationToken);
	}

	public ValueTask WriteAsync(string message, CancellationToken cancellationToken)
	{
		Guard.Against.NullOrEmpty(message);

		return _channel.Writer.WriteAsync(message, cancellationToken);
	}
}