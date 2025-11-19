using System.Text.Json;

namespace Conectus.Members.Infra.CrossCutting.Commons.Extensions
{
    public class JsonSnakeCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
            => name.ToSnakeCase();
    }
}
