using TranslationManagement.Core.Enums;

namespace TranslationManagement.Core.Dto;

public class TranslationJobDto
{
	public int Id { get; }
	public string CustomerName { get; }
	public ETranslationJobStatus Status { get; }
	public string OriginalContent { get; }
	public string TranslatedContent { get; }
	public double Price { get; set; }

	public TranslationJobDto(
		int id,
		string customerName,
		ETranslationJobStatus status,
		string originalContent,
		string translatedContent,
		double price)
	{
		Id = id;
		CustomerName = customerName;
		Status = status;
		OriginalContent = originalContent;
		TranslatedContent = translatedContent;
		Price = price;
	}
}