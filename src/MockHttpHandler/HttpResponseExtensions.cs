using System.Net.Http;

namespace MockHttpHandler
{
    public static class HttpResponseExtensions
    {
        public static HttpResponseMessage WithContent(this HttpResponseMessage message, string content)
        {
            if (content is null)
            {
                message.Content = null;
            }
            else
            {
                message.Content = new StringContent(content);
            }

            return message;
        }
    }
}