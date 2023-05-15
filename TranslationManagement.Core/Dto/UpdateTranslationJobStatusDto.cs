using TranslationManagement.Core.Enums;

namespace TranslationManagement.Core.Dto;

public class UpdateTranslationJobStatusDto
{
	public int TranslatorId { get; }

	public ETranslationJobStatus Status { get; }

	public UpdateTranslationJobStatusDto(int translatorId, ETranslationJobStatus status)
	{
		TranslatorId = translatorId;
		Status = status;
	}
}