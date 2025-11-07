using Conectus.Members.UnitTests.Common;

namespace Conectus.Members.UnitTests.Domain.ValueObject
{
    public class AddressTestFixture : BaseFixture
    {
        public string GetValidAddressStreet()
        {
            var addressStreet = "";
            while (addressStreet.Length < 3)
                addressStreet = Faker.Address.StreetName();
            if (addressStreet.Length > 100)
                addressStreet = addressStreet[..100];
            return addressStreet;
        }

        public string GetValidAddressDistrict()
        {
            var addressDistrict = "";
            while (addressDistrict.Length < 2)
                addressDistrict = Faker.Address.County();
            if (addressDistrict.Length > 50)
                addressDistrict = addressDistrict[..50];
            return addressDistrict;
        }

        public string GetValidAddressCity()
        {
            var addressCity = "";
            while (addressCity.Length < 2)
                addressCity = Faker.Address.City();
            if (addressCity.Length > 50)
                addressCity = addressCity[..50];
            return addressCity;
        }
        public string GetValidAddressState()
        {
            var addressCity = "";
            while (addressCity.Length < 2)
                addressCity = Faker.Address.CityPrefix();
            if (addressCity.Length > 2)
                addressCity = addressCity[..2];
            return addressCity;
        }

        public string GetValidAddressZipCode()
        {
            var addressCity = "";
            while (addressCity.Length < 8)
                addressCity = Faker.Address.ZipCode();
            if (addressCity.Length > 10)
                addressCity = addressCity[..10];
            return addressCity;
        }

        public double GetValidAddressLatitude() =>
             Faker.Random.Number(-90, 90);

        public double GetValidAddressLongitude() =>
                 Faker.Random.Number(-180, 180);
    }


    [CollectionDefinition(nameof(AddressTestFixture))]
    public class AddressTestFixtureCollection
        : ICollectionFixture<AddressTestFixture>
    {
    }
}
