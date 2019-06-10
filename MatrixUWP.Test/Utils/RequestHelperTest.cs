using MatrixUWP.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MatrixUWP.Test.Utils
{
    [TestClass]
    public class RequestHelperTest
    {
        [TestMethod]
        public async Task GetAsyncTest()
        {
            var stream = new InMemoryRandomAccessStream();
            var result = await RequestHelper.GetAsync("/", null, stream);
            Assert.IsTrue(result);

            return;
        }
    }
}
