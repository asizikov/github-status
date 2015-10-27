using System;
using Xunit;

namespace GitHub.Status.Service.Tests
{
    public sealed class StatusServiceTests
    {
        public sealed class Constructor
        {
            [Fact]
            public void ThrowsWhenNoConfigurationProvided()
            {
                Assert.Throws<ArgumentNullException>(() => new StatusService(null, null));
            }
        }
    }
}