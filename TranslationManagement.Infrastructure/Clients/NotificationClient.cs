using Ardalis.GuardClauses;
using External.ThirdParty.Services;
using FluentResults;
using TranslationManagement.Abstractions.Infrastructure;
using TranslationManagement.Infrastructure.Abstractions;

namespace TranslationManagement.Infrastructure.Clients;

internal class NotificationClient : INotificationClient
{
	private readonly INotificationService _notificationService;
	private readonly IClientPolicy _policy;

	public NotificationClient(INotificationService notificationService, IClientPolicy policy)
	{
		_notificationService = Guard.Against.Null(notificationService);
		_policy = Guard.Against.Null(policy);
	}

	public async Task<Result> SendAsync(string message, CancellationToken cancellationToken)
	{
		Guard.Against.NullOrEmpty(message);

		var result = await _policy.ImmediateRetry.ExecuteAsync(()
			=> _notificationService.SendNotification(message));

		return Result.FailIf(result == false, "Failed sending notification via external service");
	}
}