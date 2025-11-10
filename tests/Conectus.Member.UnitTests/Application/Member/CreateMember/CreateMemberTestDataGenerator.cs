
namespace Conectus.Members.UnitTests.Application.Member.CreateMember
{
    public class CreateMemberTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidCreateMemberInputs(int times = 12)
        {
            var fixture = new CreateMemberTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 8;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    //case 0:
                    //    invalidInputsList.Add(new object[] {
                    //    fixture.GetInvalidInputShortFirstName(),
                    //    "Name should be at least 3 characters long"
                    //});
                    //    break;
                    //case 1:
                    //    invalidInputsList.Add(new object[] {
                    //    fixture.GetInvalidInputFirstName(),
                    //    "Name should be less or equal 50 characters long"
                    //});
                    //    break;
                    default:
                        break;
                }
            }


            return invalidInputsList;
        }
    }
}
