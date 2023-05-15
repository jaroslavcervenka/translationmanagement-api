using FluentValidation;
using TranslationManagement.Application.Messaging.Commands.Jobs;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Api.Validators;

public sealed class CreateJobCommandValidator: AbstractValidator<CreateJobCommand>
{
	public CreateJobCommandValidator()
	{
		RuleFor(x => x.Payload.CustomerName)
			.NotEmpty();

		RuleFor(x => x.Payload.OriginalContent)
			.NotEmpty();
	}
}