using System;
using Xunit;

namespace GitHub.Status.Service
{
    public class StatusServiceTests
    {
        public class Constructor
        {
            [Fact]
            public void ThrowsWhenNoConfigurationProvided()
            {
                Assert.Throws<ArgumentNullException>(() => new StatusSerivce(null));
            }
        }
    }
}