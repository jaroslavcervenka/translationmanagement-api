using TranslationManagement.Core.Enums;

namespace TranslationManagement.Core.Dto;

public class TranslatorDto
{
	public int Id { get; }
	public string Name { get;  }
	public string HourlyRate { get;  }
	public ETranslatorStatus Status { get; }
	public string CreditCardNumber { get;  }

	public TranslatorDto(int id, string name, string hourlyRate, ETranslatorStatus status, string creditCardNumber)
	{
		Id = id;
		Name = name;
		HourlyRate = hourlyRate;
		Status = status;
		CreditCardNumber = creditCardNumber;
	}
}