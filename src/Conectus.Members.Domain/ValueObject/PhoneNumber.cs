using Conectus.Members.Domain.Validation;
using System.Text.RegularExpressions;

namespace Conectus.Members.Domain.ValueObject
{
    public sealed class PhoneNumber : SeedWork.ValueObject
    {
        public string Value { get; }

        private static readonly int[] ValidDDDs = new int[]
        {
            11, 12, 13, 14, 15, 16, 17, 18, 19,
            21, 22, 24, 27, 28, 31, 32, 33, 34, 35,
            37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49,
            51, 53, 54, 55, 61, 62, 63, 64, 65, 66, 67,
            68, 69, 71, 73, 74, 75, 77, 79, 81, 82, 83,
            84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95,
            96, 97, 98, 99
        };

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                DomainValidation.NotNullOrEmpty<PhoneNumber>(value, nameof(PhoneNumber));

            var normalized = Normalize(value);

            if (!IsValid(normalized))
                DomainValidation.InvalidAtritibute<PhoneNumber>(nameof(PhoneNumber));

            Value = normalized;
        }

        private static string Normalize(string value)
        {
            var digits = Regex.Replace(value, @"\D", "");

            if (!digits.StartsWith("55"))
                digits = "55" + digits;

            return $"+{digits}";
        }

        private static bool IsValid(string value)
        {
            if (!value.StartsWith("+55"))
                return false;

            var digits = value.Substring(1);

            if (!Regex.IsMatch(digits, @"^\d+$"))
                return false;

            if (digits.Length != 12 && digits.Length != 13)
                return false;

            if (!int.TryParse(digits.Substring(2, 2), out int ddd))
                return false;

            if (!ValidDDDs.Contains(ddd))
                return false;

            return true;
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
