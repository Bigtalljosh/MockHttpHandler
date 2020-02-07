using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace MockHttpHandler.Tests
{
    public class MockHttpResponseTests
    {
        [Test]
        public void New_WithNullContent_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => new MockHttpResponse(System.Net.HttpStatusCode.OK, null));
        }

        [Test]
        public void Execute_WithNullRequest_DoesNotThrowException()
        {
            var mockResponse = new MockHttpResponse(System.Net.HttpStatusCode.OK, null);

            Assert.DoesNotThrow(() => mockResponse.Execute(null));
        }

        [Test]
        public async Task Execute_WithStatusCode504_SetsStatusCode()
        {
            var statusCode = (HttpStatusCode)504;
            var mockResponse = new MockHttpResponse(statusCode);

            System.Net.Http.HttpResponseMessage response = await mockResponse.Execute(null);

            Assert.AreEqual(statusCode, response.StatusCode);
        }

        [Test]
        public async Task Execute_WithContent_SetsContent()
        {
            var content = "Sample content";
            var mockResponse = new MockHttpResponse(0, content);

            System.Net.Http.HttpResponseMessage response = await mockResponse.Execute(null);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(content, responseContent);
        }
    }
}