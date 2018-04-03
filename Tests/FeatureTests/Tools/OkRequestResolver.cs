using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FeatureTests.Tools
{
    public class OkRequestResolver
    {
        public (bool isOkResult, string expected, string result) GetOkResult<T> (T expected, IActionResult result)
        {
            if (!(result is OkObjectResult okResult)) return (false, string.Empty, string.Empty);

            var obj1Str = JsonConvert.SerializeObject(expected);
            var obj2Str = JsonConvert.SerializeObject(okResult.Value);

            return (true, obj2Str, obj1Str);
        }
    }
}