using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MockHttpHandler
{
    public class MockHttpResponse
    {
        public Task<HttpResponseMessage> Execute(HttpRequestMessage _)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                Content = Content,
                StatusCode = StatusCode
            });
        }

        public HttpContent Content { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public MockHttpResponse(HttpStatusCode statusCode, string content = null)
        {
            if (content != null)
            {
                Content = new StringContent(content);
            }

            StatusCode = statusCode;
        }

        public MockHttpResponse(int statusCode, string content = null) : this((HttpStatusCode)statusCode, content)
        {
        }
    }
}
