using Conectus.Members.Domain.ValueObject;

namespace Conectus.Members.Application.UseCases.Member.Common
{
    public record AddressDto
    {
        public required string Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }
        public required string District { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public static AddressDto FromDomain(Address address) =>
            new AddressDto
            {
                Street = address.Street,
                Number = address.Number,
                Complement = address.Complement,
                District = address.District,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            };

        public static Address ToDomain(AddressDto dto) => new Address(
                dto.Street,
                dto.Number,
                dto.Complement,
                dto.District,
                dto.City,
                dto.State,
                dto.ZipCode,
                dto.Latitude, 
                dto.Longitude);
    }
}