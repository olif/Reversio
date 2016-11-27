using FluentAssertions;
using Xunit;

namespace Reversio.Server.UnitTest
{
    public class Class1
    {
        public Class1()
        {
        }

        [Fact]
        public void Test()
        {
            var message = new Message("test");
            var json = message.ToJson();
            json.Should().NotBeEmpty();
        }
    }
}
