using System;
using System.Net;

namespace ApplicationConcept
{
    public delegate void OnResponse(HttpRerquestEventArgs e);

    public delegate void OnError(HttpRerquestEventArgs e);

    public delegate void OnUnAuthorized(HttpStatusCode code);

    public class HttpResponseHandler
    {
        public event OnResponse OnResponse;

        public event OnError OnError;

        public event OnUnAuthorized OnUnAuthorized;

        public virtual void Response(string response)
        {
            OnResponse?.Invoke(new HttpRerquestEventArgs
            {
                Response = response
            });
        }

        public virtual void Error(Exception exception)
        {
            OnError?.Invoke(new HttpRerquestEventArgs
            {
                Exception = exception
            });
        }

        public virtual void UnAuthorized(HttpStatusCode code)
        {
            OnUnAuthorized?.Invoke(code);
        }
    }
}
