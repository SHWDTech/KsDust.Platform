﻿using System.Net;

namespace ApplicationConcept
{
    public class HttpResponseAsyncResult
    {
        public HttpWebRequest Request { get; }

        public HttpResponseHandler Handler { get; }

        public HttpResponseAsyncResult(HttpWebRequest request, HttpResponseHandler handler)
        {
            Request = request;
            Handler = handler;
        }
    }
}
