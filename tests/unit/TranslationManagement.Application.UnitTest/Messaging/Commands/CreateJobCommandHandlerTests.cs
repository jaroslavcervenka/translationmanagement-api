using AutoMapper;
using FluentAssertions;
using Moq;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Application.Abstractions;
using TranslationManagement.Application.Messaging.Commands.Jobs;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Application.UnitTest.Messaging.Commands;

public class CreateJobCommandHandlerTests
{
	private readonly CreateJobCommandHandler _sut;
	private readonly Mock<IUnitOfWork<TranslationJob>> _uowMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly Mock<ITranslationPriceCalculator> _priceCalculatorMock;
	private readonly Mock<INotificationProvider> _notificationProviderMock;
	private readonly CreateJobCommand _request;
	private readonly CancellationToken _cancellationToken;

	public CreateJobCommandHandlerTests()
	{
		_uowMock = new Mock<IUnitOfWork<TranslationJob>>(MockBehavior.Strict);
		_mapperMock = new Mock<IMapper>(MockBehavior.Strict);
		_priceCalculatorMock = new Mock<ITranslationPriceCalculator>(MockBehavior.Strict);
		_notificationProviderMock = new Mock<INotificationProvider>(MockBehavior.Strict);
		_sut = new CreateJobCommandHandler(
			_uowMock.Object,
			_mapperMock.Object,
			_priceCalculatorMock.Object,
			_notificationProviderMock.Object);
		var dto = new AddTranslationJobDto("Customer 1", "original content");
		_request = new CreateJobCommand(dto);
		_cancellationToken = CancellationToken.None;
	}

	[Fact]
	public void Handle_AddedToRepository_SuccessResult()
	{
		//arrange

		//act

		//assert
	}

	[Fact]
	public void Handle_RepositoryFail_FailedResult()
	{
		//arrange

		//act

		//assert
	}
}