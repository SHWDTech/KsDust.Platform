using System.Net;

namespace ApplicationConcept
{
    public class HttpResponseAsyncResult
    {
        public HttpWebRequest Request { get; }

        public HttpResponseHandler Handler { get; }

        public HttpRequestParamState State { get; }

        public HttpResponseAsyncResult(HttpWebRequest request, HttpResponseHandler handler, HttpRequestParamState state)
        {
            Request = request;
            Handler = handler;
            State = state;
        }
    }
}
