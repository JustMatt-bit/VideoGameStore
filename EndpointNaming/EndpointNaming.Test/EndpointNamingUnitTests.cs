using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = EndpointNaming.Test.CSharpCodeFixVerifier<
    EndpointNaming.EndpointNamingAnalyzer,
    EndpointNaming.EndpointNamingCodeFixProvider>;
using Microsoft.AspNetCore.Mvc;

namespace EndpointNaming.Test
{
    [TestClass]
    public class EndpointNamingUnitTest
    {
        // No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic("EndpointNaming").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        // Test for EndpointNamingAnalyzer
        [TestMethod]
        public async Task TestEndpointNamingAnalyzer()
        {
            var test = @"
    using Microsoft.AspNetCore.Mvc;

    namespace WebApplication1
    {
        public class GenresController : ControllerBase
        {
            [HttpGet]
            public IActionResult {|#0:GetGenres|}()
            {
                return Ok();
            }
        }
    }";

            var fixtest = @"
    using Microsoft.AspNetCore.Mvc;

    namespace WebApplication1
    {
        public class GenresController : ControllerBase
        {
            [HttpGet]
            public IActionResult Genres()
            {
                return Ok();
            }
        }
    }";

            var expected = VerifyCS.Diagnostic("EndpointNaming").WithLocation(0).WithArguments("GetGenres");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
