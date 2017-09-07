using System;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
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

        public override void Error(Exception exception)
        {
            base.Error(exception);
            _activity.RunOnUiThread(() =>
            {
                Toast.MakeText(_activity, "获取数据失败！", ToastLength.Long);
            });
        }

        public override void UnAuthorized(HttpStatusCode code)
        {
            base.UnAuthorized(code);
            _activity.RunOnUiThread(() =>
            {
                Toast.MakeText(_activity, "当前登录信息已失效，请重新登陆！", ToastLength.Long).Show();
                new Handler().PostDelayed(() =>
                {
                    using (var intent = new Intent(_activity, typeof(LoginActivity)))
                    {
                        _activity.StartActivity(intent);
                        _activity.Finish();
                    }
                }, 2000);
            });
        }
    }
}