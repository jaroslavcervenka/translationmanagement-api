using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Enums;

namespace TranslationManagement.Application.UnitTest.Fixtures;

internal static class JobFixtures
{
	public static TranslationJob NewTranslationJob(int id, ETranslationJobStatus status)
	{
		return new TranslationJob(
			id,
			"Customer 1",
			status,
			"Original content",
			string.Empty,
			0);
	}
}