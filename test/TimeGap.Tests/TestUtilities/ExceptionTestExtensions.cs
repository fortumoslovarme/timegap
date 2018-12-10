using System;
using Xunit;

namespace TimeGap.Tests.TestUtilities
{
    public static class ExceptionTestExtensions
    {
        public static void Verify<T>(this Exception exception, string expectedMessage) where T : Exception
        {
            Assert.NotNull(exception);
            Assert.IsType<T>(exception);
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
