using Xunit;
using GitHub.Status.Service;

namespace GitHub.Status.Service
{
    public class StatusSerivceTests
    {
        public class Constructor {
        [Fact]
        public void ThrowsWhenNoConfigurationProvided()
        {
            
            Assert.Throws<ArgumentNullException>(() => new StatusService(null));
        }    
    }
}