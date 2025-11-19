using Newtonsoft.Json.Serialization;

namespace Conectus.Members.Infra.CrossCutting.Commons.Extensions
{
    public static class StringSnakeCaseExtension
    {
        private readonly static NamingStrategy _snakeCaseNamingStrategy =
            new SnakeCaseNamingStrategy();

        public static string ToSnakeCase(this string stringToConvert)
        {
            ArgumentNullException
                .ThrowIfNull(stringToConvert, nameof(stringToConvert));
            return _snakeCaseNamingStrategy
                .GetPropertyName(stringToConvert, false);
        }
    }
}
