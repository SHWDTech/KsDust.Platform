using System;

namespace ApplicationConcept
{
    public delegate void OnResponse(HttpRerquestEventArgs e);

    public delegate void OnError(HttpRerquestEventArgs e);

    public class HttpResponseHandler
    {
        public event OnResponse OnResponse;

        public event OnError OnError;

        public void Response(string response)
        {
            OnResponse?.Invoke(new HttpRerquestEventArgs
            {
                Response = response
            });
        }

        public void Error(Exception exception)
        {
            OnError?.Invoke(new HttpRerquestEventArgs()
            {
                Exception = exception
            });
        }
    }
}
