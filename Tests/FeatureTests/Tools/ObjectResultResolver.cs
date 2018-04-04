using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FeatureTests.Tools
{
    public class ObjectResultResolver
    {
        public (ObjectResult objectResult, string expected, string result)? GetObjectResult<T> (T expected, IActionResult result)
        {
            if (!(result is ObjectResult objectResult)) return null;

            var expectedStr = JsonConvert.SerializeObject(expected);
            var resultStr = JsonConvert.SerializeObject(objectResult.Value);

            return (objectResult, expectedStr, resultStr);
        }
    }
}