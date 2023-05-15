using Polly;
using Polly.Retry;
using TranslationManagement.Infrastructure.Abstractions;

namespace TranslationManagement.Infrastructure.Policies;

internal sealed class ClientPolicy : IClientPolicy
{
	private const int RetryCount = 3;

	public AsyncRetryPolicy<bool> ImmediateRetry { get;}

	public ClientPolicy()
	{
		ImmediateRetry =
			Policy<bool>
				.Handle<ApplicationException>()
				.RetryAsync(RetryCount);
	}
}