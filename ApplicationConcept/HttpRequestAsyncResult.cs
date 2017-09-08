using System.Net;
using System.Text;

namespace ApplicationConcept
{
    public class HttpRequestAsyncState
    {
        public HttpWebRequest Request { get; }

        public StringBuilder BodyParamters { get; }

        public HttpResponseHandler Handler { get; }

        public HttpRequestParamState State { get; }

        public HttpRequestAsyncState(HttpWebRequest request, StringBuilder bodyParamters, HttpResponseHandler handler, HttpRequestParamState state)
        {
            Request = request;
            BodyParamters = bodyParamters;
            Handler = handler;
            State = state;
        }
    }
}
