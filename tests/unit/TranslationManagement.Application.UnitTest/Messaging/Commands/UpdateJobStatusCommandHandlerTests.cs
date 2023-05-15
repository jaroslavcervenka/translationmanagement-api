using AutoMapper;
using FluentAssertions;
using FluentResults;
using MediatR;
using Moq;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Application.Messaging.Commands.Jobs;
using TranslationManagement.Application.Messaging.Queries.Translators;
using TranslationManagement.Application.UnitTest.Fixtures;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;
using TranslationManagement.Core.Enums;
using TranslationManagement.Core.Results;

namespace TranslationManagement.Application.UnitTest.Messaging.Commands;

public class UpdateJobStatusCommandHandlerTests
{
	const int TranslationJobId = 1;
	const int TranslatorId = 2;

	private readonly UpdateJobStatusCommandHandler _sut;
	private readonly Mock<IUnitOfWork<TranslationJob>> _uowMock;
	private readonly Mock<IRequestHandler<GetTranslatorQuery, Result<Translator>>> _getTranslatorHandlerMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly Mock<IRepository<TranslationJob>> _repositoryMock;
	private readonly CancellationToken _cancellationToken = CancellationToken.None;

	public UpdateJobStatusCommandHandlerTests()
	{
		_repositoryMock = new Mock<IRepository<TranslationJob>>(MockBehavior.Strict);
		_uowMock = new Mock<IUnitOfWork<TranslationJob>>(MockBehavior.Strict);
		_uowMock.Setup(x => x.Repository()).Returns(_repositoryMock.Object);
		_getTranslatorHandlerMock =
			new Mock<IRequestHandler<GetTranslatorQuery, Result<Translator>>>(MockBehavior.Strict);
		_mapperMock = new Mock<IMapper>(MockBehavior.Strict);
		_sut = new UpdateJobStatusCommandHandler(
			_uowMock.Object,
			_getTranslatorHandlerMock.Object,
			_mapperMock.Object);
	}

	[Fact]
	public async Task Handle_CertifiedTranslatorChangeStatusFromNewToInProgress_SuccessResult()
	{
		//arrange
		var job = JobFixtures.NewTranslationJob(TranslationJobId, ETranslationJobStatus.New);
		var jobDto = new TranslationJobDto(job.Id, job.CustomerName, job.Status, job.OriginalContent, job.TranslatedContent, job.Price);
		var certifiedTranslator = new Translator(TranslatorId, "Translator 1", "10", ETranslatorStatus.Certified, "card number");
		const ETranslationJobStatus newStatus = ETranslationJobStatus.InProgress;
		var request = new UpdateJobStatusCommand(TranslationJobId, TranslatorId, newStatus);

		_repositoryMock.Setup(x => x.GetAsync(TranslationJobId, _cancellationToken))
			.ReturnsAsync(Result.Ok(job));
		_getTranslatorHandlerMock.Setup(x => x.Handle(
			It.Is<GetTranslatorQuery>(p => p.TranslatorId == TranslatorId), _cancellationToken))
			.ReturnsAsync(Result.Ok(certifiedTranslator));
		_uowMock.Setup(x => x.CommitAsync(_cancellationToken))
			.ReturnsAsync(1);
		_mapperMock.Setup(x => x.Map<TranslationJobDto>(job))
			.Returns(jobDto);

		//act
		var result = await _sut.Handle(request, _cancellationToken);

		//assert
		result.IsSuccess.Should().BeTrue();
		job.Status.Should().Be(newStatus, "Should set new status from request");
		_uowMock.Verify(x => x.CommitAsync(_cancellationToken), Times.Once,
			"Should commit update in repository");
	}

	[Theory]
	[InlineData(ETranslatorStatus.Applicant)]
	[InlineData(ETranslatorStatus.Deleted)]
	public async Task Handle_NotCertifiedTranslator_FailedResult(ETranslatorStatus translatorStatus)
	{
		//arrange
		var notCertifiedTranslator = new Translator(TranslatorId, "Translator 1", "10", translatorStatus, "card number");
		var request = new UpdateJobStatusCommand(TranslationJobId, TranslatorId, ETranslationJobStatus.InProgress);
		var expectedError = $"Translator '{TranslatorId}' is not certified.";

		_getTranslatorHandlerMock.Setup(x => x.Handle(
			It.Is<GetTranslatorQuery>(p => p.TranslatorId == TranslatorId), _cancellationToken))
			.ReturnsAsync(notCertifiedTranslator);

		//act
		var result = await _sut.Handle(request, _cancellationToken);

		//assert
		result.IsFailed.Should().BeTrue("Should fail when translator is not certified");
		result.HasError<BadRequestError>().Should().BeTrue("Should contain bad request error");
		var error = result.Errors.First();
		error.Message.Should().Be(expectedError, "Should be error about not certified translator");
	}

	[Fact]
	 public async Task Handle_CertifiedTranslatorSetsNotAllowedStatus_FailedResult()
	 {
	 	//arrange
	    var certifiedTranslator = new Translator(TranslatorId, "Translator 1", "10", ETranslatorStatus.Certified, "card number");
	    var request = new UpdateJobStatusCommand(TranslationJobId, TranslatorId, ETranslationJobStatus.Completed);
	    var job = JobFixtures.NewTranslationJob(TranslationJobId, ETranslationJobStatus.Completed);
	    const string expectedError = "Change job status is not allowed";


	    _repositoryMock.Setup(x => x.GetAsync(TranslationJobId, _cancellationToken))
		    .ReturnsAsync(Result.Ok(job));
	    _getTranslatorHandlerMock.Setup(x => x.Handle(
			    It.Is<GetTranslatorQuery>(p => p.TranslatorId == TranslatorId), _cancellationToken))
		    .ReturnsAsync(certifiedTranslator);

	 	//act
	    var result = await _sut.Handle(request, _cancellationToken);

	    //assert
	    result.IsFailed.Should().BeTrue("Should fail when change status to not allowed value");
	    result.HasError<BadRequestError>().Should().BeTrue();
	    var error = result.Errors.First();
	    error.Message.Should().Be(expectedError);
	 }
}