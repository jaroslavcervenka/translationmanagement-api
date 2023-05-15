using System.Text.Json;

namespace TranslationManagement.Api.Converters;

public class UpperCaseNamingPolicy : JsonNamingPolicy
{
	public override string ConvertName(string name)
	{
		return string.IsNullOrEmpty(name) ? name : name.ToUpperInvariant();
	}
}