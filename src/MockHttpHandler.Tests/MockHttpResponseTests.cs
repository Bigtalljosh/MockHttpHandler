using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MockHttpHandler.Tests
{
    public class MockHttpResponseTests
    {
        [Test]
        public void New_WithNullContent_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => new HttpResponseMessage(HttpStatusCode.OK).WithContent(null));
        }

        [Test]
        public async Task Execute_WithContent_SetsContent()
        {
            var content = "Sample content";
            var response = new HttpResponseMessage(0).WithContent(content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(content, responseContent);
        }
    }
}