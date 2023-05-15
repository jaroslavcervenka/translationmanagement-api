using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Application.Abstractions;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Application.Messaging.Commands.Jobs;

public class CreateJobCommand : IRequest<Result<TranslationJobDto>>
{
	public AddTranslationJobDto Payload { get; }

	public CreateJobCommand(AddTranslationJobDto payload)
	{
		Payload = Guard.Against.Null(payload);
	}
}

public class CreateJobCommandHandler
	: IRequestHandler<CreateJobCommand, Result<TranslationJobDto>>
{
	private readonly IUnitOfWork<TranslationJob> _uow;
	private readonly IMapper _mapper;
	private readonly ITranslationPriceCalculator _priceCalculator;
	private readonly INotificationProvider _notificationProvider;

	public CreateJobCommandHandler(
		IUnitOfWork<TranslationJob> uow,
		IMapper mapper,
		ITranslationPriceCalculator priceCalculator,
		INotificationProvider notificationProvider)
	{
		_uow = Guard.Against.Null(uow);
		_mapper = Guard.Against.Null(mapper);
		_priceCalculator = Guard.Against.Null(priceCalculator);
		_notificationProvider = Guard.Against.Null(notificationProvider);
	}


	public async Task<Result<TranslationJobDto>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var dto = request.Payload;
		var price = _priceCalculator.GetPrice(dto.OriginalContent.Length);
		var newJob = new TranslationJob(dto.CustomerName, dto.OriginalContent, price);
		var addResult = await _uow.Repository().AddAsync(newJob, cancellationToken);

		if (addResult.IsFailed)
		{
			return addResult.ToResult();
		}

		await _uow.CommitAsync(cancellationToken);
		await _notificationProvider.NewJobCreatedAsync(addResult.Value.Id, cancellationToken);

		return Result.Ok(_mapper.Map<TranslationJobDto>(addResult.Value));
	}
}