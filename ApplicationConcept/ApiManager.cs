using System;
using System.IO;
using System.Net;
using System.Text;

namespace ApplicationConcept
{
    public static class ApiManager
    {
        private static readonly string ApiServer = "http://ksdust.shweidong.com:8090";

        public static readonly string Token = $"{ApiServer}/token";

        public static readonly string Map = $"{ApiServer}/api/Map";

        public static readonly string DeviceCurrent = $"{ApiServer}/api/DeviceCurrent";

        public static readonly string DeviceHistoryData = $"{ApiServer}/api/HistoryData";

        public static readonly string DistrictAvg = $"{ApiServer}/api/DistrictAvg";

        public static readonly string DistrictDetail = $"{ApiServer}/api/DistrictDetail";

        public static readonly string CascadeElement = $"{ApiServer}/api/CascadeElement";

        public static readonly string Search = $"{ApiServer}/api/Search";

        public static readonly string StatisticsDetial = $"{ApiServer}/api/StatisticsDetail";

        public const string HttpMethodPost = "POST";

        public const string HttpMethodGet = "GET";

        public static void StartRequest(string api, string method, XHttpRequestParamters paramter, HttpResponseHandler handler)
        {
            var request = (HttpWebRequest)WebRequest.Create(api);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            foreach (var headerString in paramter.HeaderStrings)
            {
                request.Headers[headerString.Key] = headerString.Value;
            }

            var builder = new StringBuilder();
            foreach (var bodyParamter in paramter.BodyParamters)
            {
                builder.AppendFormat("&{0}={1}", bodyParamter.Key, bodyParamter.Value);
            }
            builder.Remove(0, 1);

            request.BeginGetRequestStream(PostCallBack, new HttpRequestAsyncState(request, builder, handler));
        }

        private static void PostCallBack(IAsyncResult asynchronousResult)
        {
            var asyncResult = (HttpRequestAsyncState)asynchronousResult.AsyncState;
            try
            {
                var postStream = asyncResult.Request.EndGetRequestStream(asynchronousResult);
                var byteArray = Encoding.UTF8.GetBytes(asyncResult.BodyParamters.ToString());
                postStream.Write(byteArray, 0, asyncResult.BodyParamters.Length);

                asyncResult.Request.BeginGetResponse(ReadCallBack, new HttpResponseAsyncResult(asyncResult.Request, asyncResult.Handler));
            }
            catch (Exception ex)
            {
                asyncResult.Handler.Error(ex);
            }
        }

        private static void ReadCallBack(IAsyncResult asynchronousResult)
        {
            var asyncResult = (HttpResponseAsyncResult) asynchronousResult.AsyncState;
            try
            {
                var reponse = asyncResult.Request.EndGetResponse(asynchronousResult);
                var stream = reponse.GetResponseStream();
                if (stream == null)
                {
                    asyncResult.Handler.Response(string.Empty);
                    return;
                }
                using (var reader = new StreamReader(stream))
                {
                    var responseStr = reader.ReadToEnd();
                    asyncResult.Handler.Response(responseStr);
                }
            }
            catch (Exception ex)
            {
                asyncResult.Handler.Error(ex);
            }
        }

        private const string ParamterNameGrantType = "grant_type";

        private const string ParamterGrantTypePassword = "password";

        private const string ParamterGrantTypeRefreshToken = "refresh_token";

        private const string ParamterNameRefreshToken = "refresh_token";

        private const string ParamterNameUserName = "username";

        private const string ParamterNamePassword = "password";

        private const string ParamterNameClientId = "client_id";

        private const string ParamterClientId = "nativeAndroid";

        private const string ParamterNameClientSecret = "client_secret";

        private const string ParamterClientSecret = "7E0C829B32A6";

        private const string ParamterNameAuthorization = "Authorization";

        private const string ParamterNameSearchName = "searchName";

        private const string ParamterNameProjectType = "projectType";

        private const string ParamterNameDistrict = "district";

        public static void GetTokenByPassword(string username, string password, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(ParamterNameGrantType, ParamterGrantTypePassword);
            requestParams.AddBodyParamter(ParamterNameUserName, username);
            requestParams.AddBodyParamter(ParamterNamePassword, password);
            requestParams.AddBodyParamter(ParamterNameClientId, ParamterClientId);
            requestParams.AddBodyParamter(ParamterNameClientSecret, ParamterClientSecret);
            StartRequest(Token, HttpMethodPost, requestParams, handler);
        }

        public static void GetTokenByRefreshToken(string refreshToken, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(ParamterNameGrantType, ParamterGrantTypeRefreshToken);
            requestParams.AddBodyParamter(ParamterNameRefreshToken, refreshToken);
            requestParams.AddBodyParamter(ParamterNameClientId, ParamterClientId);
            requestParams.AddBodyParamter(ParamterNameClientSecret, ParamterClientSecret);
            StartRequest(Token, HttpMethodPost, requestParams, handler);
        }

        public static void GetSearch(string serachText, string accessToken, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(ParamterNameSearchName, serachText);
            requestParams.AddHeader(ParamterNameAuthorization, "bearer " + accessToken);
            StartRequest(Search, HttpMethodPost, requestParams, handler);
        }

        public static void GetDeviceLocationByProjectType(string projectType, string accessToken, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(ParamterNameProjectType, projectType);
            requestParams.AddHeader(ParamterNameAuthorization, "bearer " + accessToken);
            StartRequest(Map, HttpMethodPost, requestParams, handler);
        }

        public static void GetDeviceLocationByDistrictId(string projectType, string districtId, string accessToken,
            HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(ParamterNameProjectType, projectType);
            requestParams.AddBodyParamter(ParamterNameDistrict, districtId);
            requestParams.AddHeader(ParamterNameAuthorization, "bearer " + accessToken);
            StartRequest(Map, HttpMethodPost, requestParams, handler);
        }
    }
}

