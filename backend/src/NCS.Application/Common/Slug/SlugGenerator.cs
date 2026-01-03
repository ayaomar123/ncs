using System.Text;
using System.Text.RegularExpressions;

namespace NCS.Application.Common.Slug;

public static partial class SlugGenerator
{
    public static string Generate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var normalized = input.Trim().ToLowerInvariant();
        normalized = DiacriticsRegex().Replace(normalized.Normalize(NormalizationForm.FormD), string.Empty);
        normalized = normalized.Normalize(NormalizationForm.FormC);

        normalized = NonSlugCharsRegex().Replace(normalized, "-");
        normalized = MultiDashRegex().Replace(normalized, "-");
        normalized = normalized.Trim('-');

        return normalized;
    }

    [GeneratedRegex("\\p{Mn}+")]
    private static partial Regex DiacriticsRegex();

    [GeneratedRegex("[^a-z0-9]+", RegexOptions.Compiled)]
    private static partial Regex NonSlugCharsRegex();

    [GeneratedRegex("-+", RegexOptions.Compiled)]
    private static partial Regex MultiDashRegex();
}
