using Conectus.Members.Domain.Validation;
using System.Text.RegularExpressions;

namespace Conectus.Members.Domain.ValueObject
{
    public sealed class PhoneNumber : SeedWork.ValueObject
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                DomainValidation.NotNullOrEmpty<PhoneNumber>(value, nameof(PhoneNumber));

            var normalized = Normalize(value);

            if (!IsValid(normalized))
                DomainValidation.InvalidAtrtibute<PhoneNumber>(nameof(PhoneNumber));

            Value = normalized;
        }


        private static string Normalize(string value)
        {
            var digits = Regex.Replace(value, @"\D", "");

            if (digits.StartsWith("55"))
                return $"+{digits}";

            return $"+55{digits}";
        }

        private static bool IsValid(string value)
        {
            return Regex.IsMatch(value, @"^\+55\d{11}$");
        }

        public override string ToString() => Value;

        protected override int GetCustomHashCode() => Value.GetHashCode();

        public override bool Equals(SeedWork.ValueObject? other)
        {
            if (other is null) return false;
            return
                other is PhoneNumber phoneNumber &&
                Value == phoneNumber.Value;
        }
    }
}
