using FluentAssertions;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Enums;

namespace TranslationManagement.Core.UnitTest.Domain;

public class TranslationJobTests
{
	[Theory]
	[InlineData(ETranslationJobStatus.New, ETranslationJobStatus.InProgress)]
	[InlineData(ETranslationJobStatus.InProgress, ETranslationJobStatus.Completed)]
	public void UpdateStatus_AllowedChange_SuccessResult(ETranslationJobStatus currentStatus, ETranslationJobStatus newStatus)
	{
		//arrange
		var sut = NewTranslationJob(currentStatus);

		//act
		var result = sut.UpdateStatus(newStatus);

		//assert
		result.IsSuccess.Should().BeTrue("Should be success result when status change is allowed");
	}

	[Theory]
	[InlineData(ETranslationJobStatus.New, ETranslationJobStatus.Completed)]
	[InlineData(ETranslationJobStatus.Completed, ETranslationJobStatus.New)]
	public void UpdateStatus_NotAllowedChange_FailedResult(ETranslationJobStatus currentStatus, ETranslationJobStatus newStatus)
	{
		//arrange
		var sut = NewTranslationJob(currentStatus);

		//act
		var result = sut.UpdateStatus(newStatus);

		//assert
		result.IsFailed.Should().BeTrue("Should be failed result when status change is not allowed");
	}

	private static TranslationJob NewTranslationJob(ETranslationJobStatus currentStatus)
	{
		return new TranslationJob(1, "Customer", currentStatus, "content", string.Empty, 10);
	}
}