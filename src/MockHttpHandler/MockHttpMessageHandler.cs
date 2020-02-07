using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MockHttpHandler
{
    using RequestHandler = Func<HttpRequestMessage, Task<HttpResponseMessage>>;

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<Uri, RequestHandler> _handlers;
        private readonly RequestHandler _defaultHandler;

        public MockHttpMessageHandler()
        {
            _handlers = new Dictionary<Uri, RequestHandler>();
            _defaultHandler = new MockHttpResponse(HttpStatusCode.NotFound).Execute;
        }

        public MockHttpMessageHandler(RequestHandler defaultHandler)
        {
            _handlers = new Dictionary<Uri, RequestHandler>();
            _defaultHandler = defaultHandler;
        }

        public MockHttpMessageHandler(MockHttpResponse defaultResponse)
        {
            _handlers = new Dictionary<Uri, RequestHandler>();

            if (defaultResponse is null)
            {
                _defaultHandler = null;
            }
            else
            {
                _defaultHandler = defaultResponse.Execute;
            }
        }

        public void RegisterResponse(Uri uri, RequestHandler handler)
        {
            _handlers.Add(uri, handler);
        }

        public void RegisterResponse(string uri, RequestHandler handler)
        {
            RegisterResponse(new Uri(uri), handler);
        }

        public void RegisterResponse(Uri uri, MockHttpResponse response)
        {
            _handlers.Add(uri, response.Execute);
        }

        public void RegisterResponse(string uri, MockHttpResponse response)
        {
            RegisterResponse(new Uri(uri), response.Execute);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var incomingUri = request.RequestUri;

            if (!_handlers.TryGetValue(incomingUri, out var handler))
            {
                if (_defaultHandler is null)
                {
                    throw new ArgumentException($"No match for URI \"{incomingUri}\" was found, and no default handler has been provider", nameof(request));
                }

                handler = _defaultHandler;
            }

            return handler(request);
        }
    }
}
