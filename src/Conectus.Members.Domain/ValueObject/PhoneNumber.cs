using System.Text.RegularExpressions;

namespace Conectus.Members.Domain.ValueObject
{
    public sealed class PhoneNumber : IEquatable<PhoneNumber>
    {
        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber Create(string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
                throw new ArgumentException("Phone number cannot be empty.", nameof(rawValue));

            var normalized = Normalize(rawValue);

            if (!IsValid(normalized))
                throw new ArgumentException($"Invalid phone number format: {rawValue}", nameof(rawValue));

            return new PhoneNumber(normalized);
        }

        private static string Normalize(string value)
        {
            return Regex.Replace(value, @"[\s\-\(\)]", "");
        }

        private static bool IsValid(string value)
        {
            return Regex.IsMatch(value, @"^\+?[1-9]\d{1,14}$");
        }

        public override string ToString() => Value;

        public override bool Equals(object obj) => Equals(obj as PhoneNumber);

        public bool Equals(PhoneNumber other)
            => other is not null && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(PhoneNumber left, PhoneNumber right)
            => Equals(left, right);

        public static bool operator !=(PhoneNumber left, PhoneNumber right)
            => !Equals(left, right);
    }
}
