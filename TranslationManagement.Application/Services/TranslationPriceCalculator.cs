using Ardalis.GuardClauses;
using TranslationManagement.Application.Abstractions;

namespace TranslationManagement.Application.Services;

internal sealed class TranslationPriceCalculator : ITranslationPriceCalculator
{
	private const double PricePerCharacter = 0.01;

	public double GetPrice(int contentLength)
	{
		Guard.Against.NegativeOrZero(contentLength);

		return contentLength * PricePerCharacter;
	}
}