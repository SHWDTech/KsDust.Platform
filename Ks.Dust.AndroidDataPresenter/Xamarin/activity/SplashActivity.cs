using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = "昆山扬尘在线监控平台", MainLauncher = true)]
    public class SplashActivity : KsDustBaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);
            // Create your application here
            Toast.MakeText(this, "登录中...", ToastLength.Short).Show();
            if (AuthticationManager.IsLoginAndTokenValid())
            {
                AuthticationManager.OnRefreshTokenFinished += OnRefreshTokenFinished;
                AuthticationManager.UpdateAccessTokenByRefreshToken();
            }
            else
            {
                using (var handler = new Handler())
                {
                    handler.PostDelayed(() =>
                    {
                        using (var intent = new Intent(this, typeof(LoginActivity)))
                        {
                            StartActivity(intent);
                            Finish();
                        }
                    }, 1500);
                }
            }
        }

        protected void OnRefreshTokenFinished(AuthticationEventArgs args)
        {
            AuthticationManager.OnRefreshTokenFinished -= OnRefreshTokenFinished;
            RunOnUiThread(() =>
            {
                if (args.AuthSuccess)
                {
                    using (var handler = new Handler())
                    {
                        handler.PostDelayed(() =>
                        {
                            using (var intent = new Intent(this, typeof(MainActivity)))
                            {
                                StartActivity(intent);
                                Finish();
                            }
                        }, 1500);
                    }
                    Toast.MakeText(this, "登陆成功，进入主页。", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "登陆信息已经过期，请重新登陆。", ToastLength.Short).Show();
                    using (var handler = new Handler())
                    {
                        handler.PostDelayed(() =>
                        {
                            using (var intent = new Intent(this, typeof(LoginActivity)))
                            {
                                StartActivity(intent);
                                Finish();
                            }
                        }, 1500);
                    }
                }
            });
        }
    }
}