namespace TranslationManagement.Core.Dto;

public class AddTranslationJobDto
{
	public string CustomerName { get; }
	public string OriginalContent { get;  }

	public AddTranslationJobDto(string customerName, string originalContent)
	{
		CustomerName = customerName;
		OriginalContent = originalContent;
	}
}