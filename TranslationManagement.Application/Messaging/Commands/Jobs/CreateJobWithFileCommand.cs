using System.Xml.Linq;
using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using MediatR;
using TranslationManagement.Abstractions.Persistance;
using TranslationManagement.Application.Abstractions;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;
using TranslationManagement.Core.Results;

namespace TranslationManagement.Application.Messaging.Commands.Jobs;

public sealed class CreateJobWithFileCommand : IRequest<Result<TranslationJobDto>>
{
	public string FileName { get; }
	public Stream FileStream { get; }

	public string Customer { get; }

	public CreateJobWithFileCommand(string fileName, Stream fileStream, string customer)
	{
		FileName = Guard.Against.NullOrEmpty(fileName);
		FileStream = Guard.Against.Null(fileStream);
		Customer = customer;
	}
}

public sealed class CreateJobWithFileCommandHandler
	: IRequestHandler<CreateJobWithFileCommand, Result<TranslationJobDto>>
{
	private const string FileExtensionTxt = "txt";
	private const string FileExtensionXml = "xml";

	private readonly IUnitOfWork<TranslationJob> _uow;
	private readonly IMapper _mapper;
	private readonly ITranslationPriceCalculator _priceCalculator;
	private readonly INotificationProvider _notificationProvider;

	public CreateJobWithFileCommandHandler(
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

	public async Task<Result<TranslationJobDto>> Handle(
		CreateJobWithFileCommand request,
		CancellationToken cancellationToken)
	{
		Guard.Against.Null(request);

		var fileExtension = Path.GetExtension(request.FileName);

		if (!IsValidExtension(fileExtension))
		{
			return Result.Fail(new BadRequestError($"Unsupported file type '{fileExtension}'"));
		}

		//TODO: refactor to TranslationJobParser
		var reader = new StreamReader(request.FileStream);
		var content = await reader.ReadToEndAsync(cancellationToken);

		if (string.IsNullOrEmpty(content))
		{
			return Result.Fail(new BadRequestError("Cannot read a file content. Content is null or empty"));
		}

		string customer = string.Empty;

		if (fileExtension == FileExtensionXml)
		{
			var xDoc = XDocument.Parse(content);
			content = xDoc.Root?.Element("Content")?.Value;
			var parsedCustomer = xDoc.Root?.Element("Customer")?.Value.Trim();

			if (string.IsNullOrEmpty(content))
			{
				return Result.Fail(new BadRequestError("Cannot read a file content. Content is null or empty"));
			}

			if (!string.IsNullOrEmpty(parsedCustomer))
			{
				customer = parsedCustomer;
			}
		}

		var price = _priceCalculator.GetPrice(content.Length);
		var newJob = new TranslationJob(customer, content, price);
		var addResult = await _uow.Repository().AddAsync(newJob, cancellationToken);

		if (addResult.IsFailed)
		{
			return addResult.ToResult();
		}

		await _uow.CommitAsync(cancellationToken);
		await _notificationProvider.NewJobCreatedAsync(addResult.Value.Id, cancellationToken);

		return Result.Ok(_mapper.Map<TranslationJobDto>(addResult.Value));
	}

	private static bool IsValidExtension(string extension)
	{
		return extension == FileExtensionTxt | extension == FileExtensionXml;
	}
}