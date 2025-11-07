using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.SeedWork;
using Conectus.Members.Domain.ValueObject;

namespace Conectus.Members.Domain.Entity
{
    public class Member : AggregateRoot
    {
        public IdentifierDocument Document { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Guid? ResponsibleId { get; set; }
        public Member? Responsible { get; set; }
        public Address Address { get; private set; }
        public bool IsMinor => DateOfBirth.AddYears(18) > DateTime.UtcNow;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Member(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            PhoneNumber phoneNumber,
            IdentifierDocument document,
            Address address,
            bool isActive = true)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Document = document;
            Address = address;

            IsActive = isActive;
            CreatedAt = DateTime.Now;

            Validate();
        }

        private void Validate()
        {
            Domain.Validation.DomainValidation.NotNullOrEmpty<Member>(FirstName, nameof(FirstName));

            Domain.Validation.DomainValidation.NotNullOrEmpty<Member>(LastName, nameof(LastName));

            Domain.Validation.DomainValidation.NotNull<Member>(DateOfBirth, nameof(DateOfBirth));
        }
    }
}
