using Ardalis.GuardClauses;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Enums;

namespace TranslationManagement.Core.Domain;

public class Translator : IEntity
{
	public int Id { get; init; }
	public string Name { get; init; }
	public string HourlyRate { get; init; }
	public ETranslatorStatus Status { get; set; }
	public string CreditCardNumber { get; init; }

	public Translator(int id, string name, string hourlyRate, ETranslatorStatus status, string creditCardNumber)
	{
		Id = id;
		Name = Guard.Against.NullOrEmpty(name);
		HourlyRate = Guard.Against.NullOrEmpty(hourlyRate);
		Status = Guard.Against.EnumOutOfRange(status);
		CreditCardNumber = Guard.Against.NullOrEmpty(creditCardNumber);
	}

	public void SetStatus(ETranslatorStatus newStatus)
	{
		Status = Guard.Against.EnumOutOfRange(newStatus);
	}
}