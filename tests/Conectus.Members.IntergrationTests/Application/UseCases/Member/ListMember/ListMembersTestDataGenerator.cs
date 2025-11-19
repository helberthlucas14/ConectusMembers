using Conectus.Members.Application.UseCases.Member.ListMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.ListMember
{
    public class ListMembersTestDataGenerator
    {
        public static IEnumerable<object[]> GetInputsWithoutAllParameter(int times = 14)
        {
            var fixture = new ListMembersTestFixture();
            var inputExample = fixture.GetExampleInput();
            for (int i = 0; i < times; i++)
            {
                switch (i % 7)
                {
                    case 0:
                        yield return new object[] {
                        new ListMembersInput()
                    };
                        break;
                    case 1:
                        yield return new object[] {
                        new ListMembersInput(inputExample.Page)
                    };
                        break;
                    case 2:
                        yield return new object[] {
                        new ListMembersInput(
                            inputExample.Page,
                            inputExample.PerPage
                        )
                    };
                        break;
                    case 3:
                        yield return new object[] {
                        new ListMembersInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search
                        )
                    };
                        break;
                    case 4:
                        yield return new object[] {
                        new ListMembersInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search,
                            inputExample.Sort
                        )
                    };
                        break;
                    case 5:
                        yield return new object[] {
                        new ListMembersInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search,
                            inputExample.Sort,
                            inputExample.Dir
                        )
                    };
                        break;
                    case 6:
                        yield return new object[] {
                        new ListMembersInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search,
                            inputExample.Sort,
                            inputExample.Dir,
                            inputExample.FiterBy
                        )
                    };
                        break;
                    case 7:
                        yield return new object[] { inputExample };
                        break;
                    default:
                        yield return new object[] {
                        new ListMembersInput()
                    };
                        break;
                }
            }
        }
    }
}
