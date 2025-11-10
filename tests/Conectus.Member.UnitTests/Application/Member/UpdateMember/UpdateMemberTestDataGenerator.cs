using Conectus.Members.UnitTests.Application.Member.CreateMember;

namespace Conectus.Members.UnitTests.Application.Member.UpdateMember
{
    public class UpdateMemberTestDataGenerator
    {
        public static IEnumerable<object[]> GetMembersToUpdate(int times = 10)
        {
            var fixture = new UpdateMemberTestFixture();
            for (int indice = 0; indice < times; indice++)
            {
                var exampleCategory = fixture.GetValidMemberExample();
                var exampleInput = fixture.GetExampleValidInput(exampleCategory.Id);
                yield return new object[] {
                exampleCategory, exampleInput
            };
            }
        }

        public static IEnumerable<object[]> GetInvalidUpdateMemberInputs(int times = 12)
        {
            var fixture = new UpdateMemberTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 8;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortFirstName(),
                        "FirstName should be at least 3 characters long"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputLongFirstName(),
                        "FirstName should be less or equal 50 characters long"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortLastName(),
                        "LastName should be at least 3 characters long"
                    });
                        break;
                    case 3:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputLongLastName(),
                        "LastName should be less or equal 50 characters long"
                    });
                        break;
                    case 4:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortFirstNameNull(),
                        "FirstName should not be empty or null"
                    });
                        break;
                    case 5:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortLastNameNull(),
                        "LastName should not be empty or null"
                    });
                        break;
                    default:
                        break;
                }
            }
            return invalidInputsList;
        }
    }
}
