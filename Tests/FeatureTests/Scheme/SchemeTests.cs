using EF_spike.Controllers;
using FeatureTests.Tools;
using Xunit;

namespace FeatureTests.Scheme
{
    public class SchemeTests
    {
        private readonly SchemeController sut;

        private readonly OkRequestResolver resolver;

        public SchemeTests()
        {
            resolver = new OkRequestResolver();
            sut = new SchemeController();
        }

        [Fact]
        public void GetSchemeIfValidPsr()
        {
            // Arrange
            var expected = new spike.Scheme.Models.Scheme{Id = 1};

            // Act
            var result = sut.Get();

            var resolvedResult = resolver.GetOkResult(expected, result);

            // Assert
            Assert.True(resolvedResult.isOkResult);
            Assert.Equal(resolvedResult.expected, resolvedResult.result);
        }
    }
}