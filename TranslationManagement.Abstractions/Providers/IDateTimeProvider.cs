namespace TranslationManagement.Abstractions.Abstractions.Providers;

/// <summary>
/// Provider for date and time operations.
/// </summary>
public interface IDateTimeProvider
{
	/// <summary>
	/// Gets a current UTC date and time.
	/// </summary>
	/// <returns></returns>
	DateTime GetUtcNow();
}