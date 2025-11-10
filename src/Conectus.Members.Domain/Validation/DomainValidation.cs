using Conectus.Members.Domain.Exceptions;
using Conectus.Members.Domain.SeedWork;


namespace Conectus.Members.Domain.Validation
{
    public static class DomainValidation
    {
        public static void NotNull<T>(object? target, string fieldName) where T : class
            => ThrowIf<T>(target is null, $"{fieldName} should not be null");

        public static void NotNull<T>(DateTime? target, string fieldName) where T : class
             => ThrowIf<T>(target is null || DateTime.MinValue == target, $"{fieldName} should not be null");

        public static void NotNullOrEmpty<T>(string? target, string fieldName) where T : class
            => ThrowIf<T>(string.IsNullOrWhiteSpace(target), $"{fieldName} should not be empty or null");

        public static void MinLength<T>(string target, int minLength, string fieldName) where T : class
            => ThrowIf<T>(target.Length < minLength, $"{fieldName} should be at least {minLength} characters long");

        public static void MaxLength<T>(string target, int maxLength, string fieldName) where T : class
            => ThrowIf<T>(target.Length > maxLength, $"{fieldName} should be less or equal {maxLength} characters long");

        public static void Between<T>(double? target, int min, int max, string fieldName) where T : class
            => ThrowIf<T>(target.HasValue && (target < min || target > max), $"{fieldName} must be between {min} and {max}.");
        public static void InvalidAtritibute<T>(string fieldName, bool target = true) where T : class
            => ThrowIf<T>(target, $"{fieldName} is invalid.");


        private static void ThrowIf<T>(bool condition, string message) where T : class
        {
            if (!condition) return;

            Exception ex = typeof(IValueObject).IsAssignableFrom(typeof(T))
                ? new ValueObjectValidationException(message)
                : new EntityValidationException(message);

            throw ex;
        }
    }
}
