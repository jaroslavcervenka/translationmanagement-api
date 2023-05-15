namespace TranslationManagement.Application.Abstractions;

public interface ITranslationPriceCalculator
{
	double GetPrice(int contentLength);
}