using TranslationManagement.Abstractions.Abstractions.Providers;

namespace TranslationManagement.Core.Providers;

internal sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTime GetUtcNow()
	{
		return DateTime.UtcNow;
	}
}