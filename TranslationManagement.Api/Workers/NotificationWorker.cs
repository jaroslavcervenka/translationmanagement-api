using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BetterHostedServices;
using TranslationManagement.Application.Abstractions;

namespace TranslationManagement.Api.Workers;

public class NotificationWorker : CriticalBackgroundService
{
	private readonly INotificationSender _sender;

	public NotificationWorker(INotificationSender sender, IApplicationEnder applicationEnder) : base(applicationEnder)
	{
		_sender = Guard.Against.Null(sender);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		return _sender.RunAsync(stoppingToken);
	}
}