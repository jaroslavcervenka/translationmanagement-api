using AutoMapper;
using TranslationManagement.Core.Domain;
using TranslationManagement.Core.Dto;

namespace TranslationManagement.Application.Mapping;

public class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<TranslationJob, TranslationJobDto>();
		CreateMap<Translator, TranslatorDto>();
	}
}