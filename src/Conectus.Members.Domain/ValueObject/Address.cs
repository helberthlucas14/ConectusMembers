using Conectus.Members.Domain.Validation;

namespace Conectus.Members.Domain.ValueObject
{
    public class Address : SeedWork.ValueObject
    {
        public string Street { get; }
        public string Number { get; }
        public string? Complement { get; }
        public string District { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public double? Latitude { get; }
        public double? Longitude { get; }
        private Address() { } // EF Core

        public Address(
            string street,
            string number,
            string? complement,
            string district,
            string city,
            string state,
            string zipCode,
            double? latitude = null,
            double? longitude = null)
        {
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            City = city;
            State = state;
            ZipCode = SanitizeZipCode(zipCode);

            Latitude = latitude;
            Longitude = longitude;

            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty<Address>(Street, nameof(Street));
            DomainValidation.MinLength<Address>(Street, 3, nameof(Street));
            DomainValidation.MaxLength<Address>(Street, 100, nameof(Street));

            DomainValidation.NotNullOrEmpty<Address>(Number, nameof(Number));
            DomainValidation.MaxLength<Address>(Number, 10, nameof(Number));

            if (!string.IsNullOrWhiteSpace(Complement))
                DomainValidation.MaxLength<Address>(Complement, 50, nameof(Complement));

            DomainValidation.NotNullOrEmpty<Address>(District, nameof(District));
            DomainValidation.MinLength<Address>(District, 2, nameof(District));
            DomainValidation.MaxLength<Address>(District, 50, nameof(District));


            DomainValidation.NotNullOrEmpty<Address>(City, nameof(City));
            DomainValidation.MinLength<Address>(City, 2, nameof(City));
            DomainValidation.MaxLength<Address>(City, 50, nameof(City));

            DomainValidation.NotNullOrEmpty<Address>(State, nameof(State));
            DomainValidation.MinLength<Address>(State, 2, nameof(State));
            DomainValidation.MaxLength<Address>(State, 50, nameof(State));


            DomainValidation.NotNullOrEmpty<Address>(ZipCode, nameof(ZipCode));
            DomainValidation.MinLength<Address>(ZipCode, 8, nameof(ZipCode));
            DomainValidation.MaxLength<Address>(ZipCode, 10, nameof(ZipCode));

            DomainValidation.Between<Address>(Latitude, -90, 90, nameof(Latitude));
            DomainValidation.Between<Address>(Longitude, -180, 180, nameof(Longitude));
        }

        private static string SanitizeZipCode(string zip)
            => !string.IsNullOrEmpty(zip) ? new string(zip.Where(char.IsDigit).ToArray()) : string.Empty;

        public bool HasLocation() => Latitude.HasValue && Longitude.HasValue;

        public override bool Equals(SeedWork.ValueObject? other)
        {
            if (other is null) return false;
            return
                other is Address address &&
                Street == address.Street &&
                Number == address.Number &&
                Complement == address.Complement &&
                District == address.District &&
                City == address.City &&
                State == address.State &&
                ZipCode == address.ZipCode &&
                Latitude == address.Latitude &&
                Longitude == address.Longitude;
        }

        protected override int GetCustomHashCode()
            => HashCode.Combine(Street, Number, Complement, District, City, State, ZipCode);
    }
}
