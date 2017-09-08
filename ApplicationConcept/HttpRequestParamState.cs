namespace ApplicationConcept
{
    public class HttpRequestParamState
    {
        public string Api { get; set; }

        public string Method { get; set; }

        public bool IsRepeat { get; set; }

        public XHttpRequestParamters Paramter { get; set; }

        public HttpResponseHandler Handler { get; set; }
    }
}
