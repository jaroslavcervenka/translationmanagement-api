using Ardalis.GuardClauses;
using FluentResults;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Core.Enums;

namespace TranslationManagement.Core.Domain;

public record TranslationJob : IEntity
{
	public int Id { get; set; }
	public string CustomerName { get; set; }
	public ETranslationJobStatus Status { get; set; }
	public string OriginalContent { get; set; }
	public string TranslatedContent { get; set; }
	public double Price { get; set; }

	public TranslationJob(
		int id,
		string customerName,
		ETranslationJobStatus status,
		string originalContent,
		string translatedContent,
		double price)
	{
		Id = id;
		CustomerName = Guard.Against.NullOrEmpty(customerName);
		Status = Guard.Against.EnumOutOfRange(status);
		OriginalContent = Guard.Against.NullOrEmpty(originalContent);
		TranslatedContent = translatedContent;
		Price = Guard.Against.Negative(price);
	}

	/// <summary>
	/// Creates a job with status New
	/// </summary>
	public TranslationJob(string customerName, string originalContent, double price)
	{
		Id = 0;
		CustomerName = customerName;
		Status = ETranslationJobStatus.New;
		OriginalContent = originalContent;
		TranslatedContent = string.Empty;
		Price = Guard.Against.NegativeOrZero(price);
	}

	public Result UpdateStatus(ETranslationJobStatus newStatus)
	{
		if (IsInvalidStatusChange(newStatus))
		{
			return Result.Fail($"Invalid status change (status: {Status}, change to: {newStatus})");
		}

		Status = newStatus;

		return Result.Ok();
	}

	private bool IsInvalidStatusChange(ETranslationJobStatus newStatus)
	{
		return (Status == ETranslationJobStatus.New && newStatus == ETranslationJobStatus.Completed) ||
			Status == ETranslationJobStatus.Completed || newStatus == ETranslationJobStatus.New;
	}
}