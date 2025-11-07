using Bogus;
using Conectus.Members.Domain.Exceptions;
using Conectus.Members.Domain.ValueObject;
using FluentAssertions;

namespace Conectus.Members.UnitTests.Domain.ValueObject
{
    [Collection(nameof(AddressTestFixture))]
    public class AddressTest
    {
        private readonly AddressTestFixture _fixture;

        public AddressTest(AddressTestFixture fixture) => _fixture = fixture;

        #region Address 
        [Fact(DisplayName = nameof(Instantiate_Address_Successfully))]
        [Trait("Domain", "Address - ValueObject")]
        public void Instantiate_Address_Successfully()
        {
            var validAddress = _fixture.GetAddress();

            var address = new Address(
                validAddress.Street,
                validAddress.Number,
                validAddress.Complement,
                validAddress.District,
                validAddress.City,
                validAddress.State,
                validAddress.ZipCode,
                validAddress.Latitude,
                validAddress.Longitude
                );

            address.Should().NotBeNull();
            address.Street.Should().Be(validAddress.Street);
            address.Number.Should().Be(validAddress.Number);
            address.Complement.Should().Be(validAddress.Complement);
            address.District.Should().Be(validAddress.District);
            address.City.Should().Be(validAddress.City);
            address.State.Should().Be(validAddress.State);
            address.ZipCode.Should().Be(validAddress.ZipCode);
            address.Latitude.Should().Be(validAddress.Latitude);
            address.Longitude.Should().Be(validAddress.Longitude);
        }

        [Fact(DisplayName = nameof(Instantiate_Address_Successfully_Without_Complement))]
        [Trait("Domain", "Address - ValueObject")]
        public void Instantiate_Address_Successfully_Without_Complement()
        {
            var validAddress = _fixture.GetAddress();

            var address = new Address(
                validAddress.Street,
                validAddress.Number,
                string.Empty,
                validAddress.District,
                validAddress.City,
                validAddress.State,
                validAddress.ZipCode,
                validAddress.Latitude,
                validAddress.Longitude
                );

            address.Should().NotBeNull();
            address.Street.Should().Be(validAddress.Street);
            address.Number.Should().Be(validAddress.Number);
            address.Complement.Should().Be(string.Empty);
            address.District.Should().Be(validAddress.District);
            address.City.Should().Be(validAddress.City);
            address.State.Should().Be(validAddress.State);
            address.ZipCode.Should().Be(validAddress.ZipCode);
            address.Latitude.Should().Be(validAddress.Latitude);
            address.Longitude.Should().Be(validAddress.Longitude);
        }

        [Fact(DisplayName = nameof(Instantiate_Address_Successfully_Without_Longitude_Latitude))]
        [Trait("Domain", "Address - ValueObject")]
        public void Instantiate_Address_Successfully_Without_Longitude_Latitude()
        {
            var validAddress = _fixture.GetAddress();

            var address = new Address(
                validAddress.Street,
                validAddress.Number,
                validAddress.Complement,
                validAddress.District,
                validAddress.City,
                validAddress.State,
                validAddress.ZipCode,
                null,
                null
                );

            address.Should().NotBeNull();
            address.Street.Should().Be(validAddress.Street);
            address.Number.Should().Be(validAddress.Number);
            address.Complement.Should().Be(validAddress.Complement);
            address.District.Should().Be(validAddress.District);
            address.City.Should().Be(validAddress.City);
            address.State.Should().Be(validAddress.State);
            address.ZipCode.Should().Be(validAddress.ZipCode);
            address.Latitude.Should().Be(null);
            address.Longitude.Should().Be(null);
        }
        #endregion

        #region Street Validations

        [Theory(DisplayName = nameof(InstantiateErrorWhenStreetIsEmpty))]
        [Trait("Domain", "Address - ValueObject")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenStreetIsEmpty(string street)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Street should not be empty or null");

        }




        [Fact(DisplayName = nameof(InstantiateErrorWhenStreetIsGreaterThan100Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenStreetIsGreaterThan100Characters()
        {
            var validAddress = _fixture.GetAddress();

            var invalidStreet = _fixture.Faker.Random.String2(101);

            Action action = () => new Address(
                 invalidStreet,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Street should be less or equal 100 characters long");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenStreetIsLessThan3Characters))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetStreetsWithLessThan3Characters), parameters: 10)]
        public void InstantiateErrorWhenStreetIsLessThan3Characters(string street)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Street should be at least 3 characters long");
        }

        public static IEnumerable<object[]> GetStreetsWithLessThan3Characters(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] {
                fixture.GetValidAddressStreet()[..(isOdd ? 1 : 2)]
            };
            }
        }
        #endregion

        #region Number Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenNumberIsEmpty))]
        [Trait("Domain", "Address - ValueObject")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNumberIsEmpty(string number)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Number should not be empty or null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNumberIsGreaterThan10Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenNumberIsGreaterThan10Characters()
        {
            var validAddress = _fixture.GetAddress();

            var invalidNumber = _fixture.Faker.Random.String2(11);

            Action action = () => new Address(
                 validAddress.Street,
                 invalidNumber,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Number should be less or equal 10 characters long");
        }
        #endregion

        #region  Complement Validations
        [Fact(DisplayName = nameof(InstantiateErrorWhenComplementIsGreaterThan50Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenComplementIsGreaterThan50Characters()
        {
            var validAddress = _fixture.GetAddress();

            var invalidComplement = _fixture.Faker.Random.String2(51);

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 invalidComplement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Complement should be less or equal 50 characters long");
        }
        #endregion

        #region District Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenDistrictIsEmpty))]
        [Trait("Domain", "Address - ValueObject")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenDistrictIsEmpty(string district)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 district,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("District should not be empty or null");
        }


        [Theory(DisplayName = nameof(InstantiateErrorWhenStreetIsLessThan3Characters))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetDistrictWithLessThan2Characters), parameters: 10)]
        public void InstantiateErrorWhenDistrictIsLessThan2Characters(string district)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 district,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("District should be at least 2 characters long");
        }

        public static IEnumerable<object[]> GetDistrictWithLessThan2Characters(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                yield return new object[] {
                fixture.GetValidAddressDistrict()[..(1)]
            };
            }
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenDistrictIsGreaterThan50Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenDistrictIsGreaterThan50Characters()
        {
            var validAddress = _fixture.GetAddress();

            var invalidDistrict = _fixture.Faker.Random.String2(51);

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 invalidDistrict,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("District should be less or equal 50 characters long");
        }
        #endregion

        #region City Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenCityIsEmpty))]
        [Trait("Domain", "Address - ValueObject")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenCityIsEmpty(string city)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 city,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("City should not be empty or null");
        }


        [Theory(DisplayName = nameof(InstantiateErrorWhenCityIsLessThan2Characters))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetCityWithLessThan2Characters), parameters: 10)]
        public void InstantiateErrorWhenCityIsLessThan2Characters(string city)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 city,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("City should be at least 2 characters long");
        }

        public static IEnumerable<object[]> GetCityWithLessThan2Characters(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                yield return new object[] {
                fixture.GetValidAddressCity()[..(1)]
            };
            }
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenCityIsGreaterThan50Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenCityIsGreaterThan50Characters()
        {
            var validAddress = _fixture.GetAddress();

            var invalidCity = _fixture.Faker.Random.String2(51);

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 invalidCity,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("City should be less or equal 50 characters long");
        }
        #endregion

        #region State Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenStateIsEmpty))]
        [Trait("Domain", "Address - ValueObject")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenStateIsEmpty(string state)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 state,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("State should not be empty or null");
        }


        [Theory(DisplayName = nameof(InstantiateErrorWhenStateIsLessThan2Characters))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetDistrictWithLessThan2Characters), parameters: 10)]
        public void InstantiateErrorWhenStateIsLessThan2Characters(string state)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 state,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("State should be at least 2 characters long");
        }

        public static IEnumerable<object[]> GetStateWithLessThan2Characters(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                yield return new object[] {
                fixture.GetValidAddressState()[..(1)]
            };
            }
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenStateIsGreaterThan50Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenStateIsGreaterThan50Characters()
        {
            var validAddress = _fixture.GetAddress();

            var invalidState = _fixture.Faker.Random.String2(51);

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 invalidState,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("State should be less or equal 50 characters long");
        }
        #endregion

        #region ZipCode Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenZipCodeIsEmpty))]
        [Trait("Domain", "Address - ValueObject")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenZipCodeIsEmpty(string zipCode)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 zipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("ZipCode should not be empty or null");
        }


        [Theory(DisplayName = nameof(InstantiateErrorWhenZipCodeIsLessThan8Characters))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetZipCodeWithLessThan8Characters), parameters: 10)]
        public void InstantiateErrorWhenZipCodeIsLessThan8Characters(string zipCode)
        {
            var address = _fixture.GetAddress();

            Action action = () => new Address(
                 address.Street,
                 address.Number,
                 address.Complement,
                 address.District,
                 address.City,
                 address.State,
                 zipCode,
                 address.Latitude,
                 address.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("ZipCode should be at least 8 characters long");
        }

        public static IEnumerable<object[]> GetZipCodeWithLessThan8Characters(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] {
                fixture.GetValidAddressZipCode()[..(isOdd ? 1 : 2)]
            };
            }
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenZipCodeIsGreaterThan10Characters))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenZipCodeIsGreaterThan10Characters()
        {
            var validAddress = _fixture.GetAddress();

            var zipCode = String.Join(null, Enumerable.Range(2, 11).Select(_ => "1")).ToString();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 zipCode,
                 validAddress.Latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("ZipCode should be less or equal 10 characters long");
        }
        #endregion

        #region Latitude Validations

        [Theory(DisplayName = nameof(InstantiateErrorWhenStreetIsLessThanNegative90Characters))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetLatitudeWithLessThanNegative90), parameters: 10)]
        public void InstantiateErrorWhenStreetIsLessThanNegative90Characters(double latitude)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 latitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Latitude must be between -90 and 90.");
        }

        public static IEnumerable<object[]> GetLatitudeWithLessThanNegative90(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                yield return new object[]
                {
                   fixture.Faker.Random.Double(-91, -200)
                };
            }
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenLatitudeIsGreaterThan90))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenLatitudeIsGreaterThan90()
        {
            var validAddress = _fixture.GetAddress();

            var invalidLatitude = (Math.Abs(_fixture.GetValidAddressLatitude()) + 90);

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                invalidLatitude,
                 validAddress.Longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Latitude must be between -90 and 90.");
        }
        #endregion

        #region Longitude Validations

        [Theory(DisplayName = nameof(InstantiateErrorWhenLongitudeIsLessThanNegative180))]
        [Trait("Domain", "Address - ValueObject")]
        [MemberData(nameof(GetLongitudeWithLessThanNegative180), parameters: 10)]
        public void InstantiateErrorWhenLongitudeIsLessThanNegative180(double longitude)
        {
            var validAddress = _fixture.GetAddress();

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 longitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Longitude must be between -180 and 180.");
        }

        public static IEnumerable<object[]> GetLongitudeWithLessThanNegative180(int numberOfTests)
        {
            var fixture = new AddressTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                double longitude = fixture.Faker.Random.Double(-179, -200);
                yield return new object[] { longitude };
            }
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenDistrictIsGreaterThan180))]
        [Trait("Domain", "Address - ValueObject")]
        public void InstantiateErrorWhenDistrictIsGreaterThan180()
        {
            var validAddress = _fixture.GetAddress();

            var invalidLongitude = _fixture.GetValidAddressLongitude() * 100;

            Action action = () => new Address(
                 validAddress.Street,
                 validAddress.Number,
                 validAddress.Complement,
                 validAddress.District,
                 validAddress.City,
                 validAddress.State,
                 validAddress.ZipCode,
                 validAddress.Latitude,
                 invalidLongitude
                 );

            action.Should().Throw<ValueObjectValidationException>()
                .WithMessage("Longitude must be between -180 and 180.");
        }
        #endregion
    }
}
