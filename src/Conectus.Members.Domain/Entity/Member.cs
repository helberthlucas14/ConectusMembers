using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.Exceptions;
using Conectus.Members.Domain.SeedWork;
using Conectus.Members.Domain.Validation;
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
        public Member()
        {
        }

        public Member(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            PhoneNumber phoneNumber,
            IdentifierDocument document,
            Address address,
            Guid? responsibleId = null,
            bool isActive = true) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Document = document;
            Address = address;

            CreatedAt = DateTime.Now;
            ResponsibleId = IsMinor ? responsibleId : null;
            IsActive = isActive;

            Validate();
        }
        public void Update
            (
             string? firstName = null,
             string? lastName = null,
             DateTime? dateOfBirth = null,
             Gender? gender = null,
             PhoneNumber? phoneNumber = null,
             IdentifierDocument? document = null,
             Address? address = null,
             Guid? responsibleId = null
            )
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            DateOfBirth = dateOfBirth ?? DateOfBirth;
            Gender = gender ?? Gender;
            PhoneNumber = phoneNumber ?? PhoneNumber;
            Document = document ?? Document;
            Address = address ?? Address;

            ResponsibleId = IsMinor ? responsibleId : null;
            Validate();
        }

        public void Active()
        {
            IsActive = true;
            Validate();
        }

        public void Desactive()
        {
            IsActive = false;
            Validate();
        }
        private void Validate()
        {
            DomainValidation.NotNullOrEmpty<Member>(FirstName, nameof(FirstName));
            DomainValidation.MinLength<Member>(FirstName, 3, nameof(FirstName));
            DomainValidation.MaxLength<Member>(FirstName, 50, nameof(FirstName));

            DomainValidation.NotNullOrEmpty<Member>(LastName, nameof(LastName));
            DomainValidation.MinLength<Member>(LastName, 3, nameof(LastName));
            DomainValidation.MaxLength<Member>(LastName, 50, nameof(LastName));

            DomainValidation.NotNull<Member>(DateOfBirth, nameof(DateOfBirth));
            DomainValidation.InvalidAtritibute<Member>(nameof(DateOfBirth), (DateOfBirth.Date > DateTime.Now.Date));

            if (IsMinor && ResponsibleId is null)
                throw new EntityValidationException("Member is a minor and needs a guardian.");

            DomainValidation.InvalidAtritibute<Member>(nameof(ResponsibleId), (IsMinor && ResponsibleId == Id));
        }
    }
}
