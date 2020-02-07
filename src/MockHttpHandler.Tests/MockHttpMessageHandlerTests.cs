﻿using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MockHttpHandler.Tests
{
    using RequestHandler = Func<HttpRequestMessage, Task<HttpResponseMessage>>;

    public class MockHttpMessageHandlerTests
    {
        private readonly Uri _defaultUri = new Uri("http://me");
        private HttpRequestMessage DefaultRequest => new HttpRequestMessage(HttpMethod.Get, _defaultUri);

        [Test]
        public void New_WithNullDefaultHandler_DoesNotThrow()
        {
            RequestHandler handler = null;
            Assert.DoesNotThrow(() => new MockHttpMessageHandler(handler));
        }

        [Test]
        public void New_WithNullDefaultResponse_DoesNotThrow()
        {
            MockHttpResponse response = null;
            Assert.DoesNotThrow(() => new MockHttpMessageHandler(response));
        }

        [Test]
        public async Task Send_WithDefaultResponseAndNoHandlers_SetsDefaultResponse()
        {
            var status = (HttpStatusCode)504;
            var content = "Sample content";

            var handler = new MockHttpMessageHandler(new MockHttpResponse(status, content));
            var client = new HttpClient(handler);

            var result = await client.SendAsync(DefaultRequest);
            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.AreEqual(status, result.StatusCode);
            Assert.AreEqual(content, resultContent);
        }

        [Test]
        public async Task Send_WithMatchedHandler_UsesMatchedHandler()
        {
            var status = (HttpStatusCode)504;
            var content = "Sample content";

            var handler = new MockHttpMessageHandler();
            handler.RegisterResponse(_defaultUri, new MockHttpResponse(status, content));
            var client = new HttpClient(handler);

            var result = await client.SendAsync(DefaultRequest);
            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.AreEqual(status, result.StatusCode);
            Assert.AreEqual(content, resultContent);
        }

        [Test]
        public async Task Send_WithUnmatchedHandler_UsesDefaultHandler()
        {
            var status = (HttpStatusCode)504;
            var content = "Sample content";

            var handler = new MockHttpMessageHandler(new MockHttpResponse(status, content));
            handler.RegisterResponse("http://random.io", new MockHttpResponse(0));
            var client = new HttpClient(handler);

            var result = await client.SendAsync(DefaultRequest);
            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.AreEqual(status, result.StatusCode);
            Assert.AreEqual(content, resultContent);
        }

        [Test]
        public void Send_WithUnmatchedHandlerAndNoDefault_ThrowsArgumentException()
        {
            MockHttpResponse nullResponse = null;
            var handler = new MockHttpMessageHandler(nullResponse);
            var client = new HttpClient(handler);

            Assert.ThrowsAsync<ArgumentException>(async () => await client.SendAsync(DefaultRequest));
        }
    }
}