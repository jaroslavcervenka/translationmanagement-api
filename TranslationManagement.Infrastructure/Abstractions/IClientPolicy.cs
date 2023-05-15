using Polly.Retry;

namespace TranslationManagement.Infrastructure.Abstractions;

internal interface IClientPolicy
{
	AsyncRetryPolicy<bool> ImmediateRetry { get;}
}