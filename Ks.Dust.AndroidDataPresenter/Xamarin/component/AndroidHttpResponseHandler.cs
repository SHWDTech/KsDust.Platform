using System.Net;
using Android.App;
using Android.Content;
using Android.Widget;
using ApplicationConcept;
using Ks.Dust.AndroidDataPresenter.Xamarin.activity;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.component
{
    public class AndroidHttpResponseHandler : HttpResponseHandler
    {
        private readonly Activity _activity;

        public AndroidHttpResponseHandler(Activity activity)
        {
            _activity = activity;
        }

        public override void UnAuthorized(HttpStatusCode code)
        {
            base.UnAuthorized(code);
            _activity.RunOnUiThread(() =>
            {
                Toast.MakeText(_activity, "当前登录信息已失效，请重新登陆！", ToastLength.Long);

                using (var intent = new Intent(_activity, typeof(LoginActivity)))
                {
                    _activity.StartActivity(intent);
                }
            });
        }
    }
}