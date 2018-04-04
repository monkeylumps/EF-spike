using EF_spike.Controllers;
using FeatureTests.Tools;
using Xunit;

namespace FeatureTests.Scheme
{
    public class SchemeTests
    {
        private readonly SchemeController sut;

        private readonly ObjectResultResolver resolver;

        public SchemeTests()
        {
            resolver = new ObjectResultResolver();
            sut = new SchemeController();
        }

        [Fact]
        public void GetSchemeIfValidPsr()
        {
            // Arrange
            var expected = new spike.Scheme.Models.Scheme{Id = 1};

            // Act
            var result = sut.Get();

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);
        }
    }
}