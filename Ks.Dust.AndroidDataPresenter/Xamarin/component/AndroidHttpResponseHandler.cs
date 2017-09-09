using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Widget;
using ApplicationConcept;
using Ks.Dust.AndroidDataPresenter.Xamarin.activity;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.component
{
    public class AndroidHttpResponseHandler : HttpResponseHandler
    {
        private readonly KsDustBaseActivity _activity;

        private HttpRequestParamState _state;

        public AndroidHttpResponseHandler(KsDustBaseActivity activity)
        {
            _activity = activity;
        }

        public override void Response(string response)
        {
            try
            {
                base.Response(response);
            }
            catch (Exception)
            {
                _activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(_activity, "获取数据失败！", ToastLength.Long);
                });
            }
        }

        public override void Error(Exception exception)
        {
            _activity.RunOnUiThread(() =>
            {
                Toast.MakeText(_activity, "获取数据失败！", ToastLength.Long);
            });
            base.Error(exception);
        }

        public override void UnAuthorized(HttpRequestParamState state)
        {
            _state = state;
            base.UnAuthorized(state);

            if (!state.IsRepeat)
            {
                _activity.AuthticationManager.OnRefreshTokenFinished += OnOnRefreshTokenFinished;
                _activity.AuthticationManager.UpdateAccessTokenByRefreshToken();
            }
            else
            {
                RedirectToLogin();
            }
        }

        private void OnOnRefreshTokenFinished(AuthticationEventArgs args)
        {
            _activity.AuthticationManager.OnRefreshTokenFinished -= OnOnRefreshTokenFinished;
            if (args.AuthSuccess)
            {
                _state.Paramter.HeaderStrings[ApiManager.ParamterNameAuthorization] =
                    $"bearer {_activity.AuthticationManager.AccessToken}";
                Task.Factory.StartNew(() =>
                {
                    ApiManager.StartRequest(_state);
                });
            }
            else
            {
                RedirectToLogin();
            }
        }

        private void RedirectToLogin()
        {
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