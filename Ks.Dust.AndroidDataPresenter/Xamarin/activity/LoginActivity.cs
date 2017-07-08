using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Android.Content;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(LoginActivity))]
    public class LoginActivity : KsDustBaseActivity
    {
        [BindView(Resource.Id.account)]
        private AutoCompleteTextView _accountView;

        [BindView(Resource.Id.password)]
        private EditText _passwordView;

        [BindView(Resource.Id.login_progress)]
        private View _progressView;

        [BindView(Resource.Id.login_form)]
        private View _loginFormView;

        private readonly AuthticationManager _authcicationManager = AuthticationManager.Instance;

        [OnClick(Resource.Id.sign_in_button)]
        void TryLogin(object sender, EventArgs e)
        {
            _accountView.SetError(string.Empty, null);
            _passwordView.SetError(string.Empty, null);

            var cancel = false;
            View focusView = null;
            if (string.IsNullOrWhiteSpace(_passwordView.Text))
            {
                _passwordView.SetError("账号不能为空", null);
                cancel = true;
                focusView = _passwordView;
            }
            if (string.IsNullOrWhiteSpace(_accountView.Text))
            {
                _accountView.SetError("账号不能为空", null);
                cancel = true;
                focusView = _accountView;
            }

            if (cancel)
            {
                focusView.RequestFocus();
            }
            else
            {
                ShowProgress(true);

                _authcicationManager.Login(_accountView.Text, _passwordView.Text);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            Cheeseknife.Bind(this);
            _authcicationManager.OnLoginFinished += LoginFinished;
            // Create your application here
        }

        private void LoginFinished(AuthticationEventArgs args)
        {
            if (!args.AuthSuccess)
            {
                _authcicationManager.Logout();
                RunOnUiThread(() =>
                {
                    ShowProgress(false);
                    Toast.MakeText(this, args.Message, ToastLength.Short).Show();
                });
            }
            else
            {
                Toast.MakeText(this, "登录成功", ToastLength.Short).Show();
                using (var intent = new Intent(this, typeof(MainActivity)))
                {
                    StartActivity(intent);
                }
                Finish();
            }
        }

        private void ShowProgress(bool show)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
            {
                var shortAnimTime = Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);
                _loginFormView.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
                _loginFormView.Animate().SetDuration(shortAnimTime).Alpha(show ? 0 : 1)
                    .SetListener(new LoginAnimatorListenerAdapter
                    {
                        IsShow = show,
                        LoginView = _loginFormView
                    });

                _progressView.Visibility = !show ? ViewStates.Gone : ViewStates.Visible;
                _loginFormView.Animate().SetDuration(shortAnimTime).Alpha(show ? 0 : 1)
                    .SetListener(new LoginAnimatorListenerAdapter
                    {
                        IsShow = !show,
                        LoginView = _progressView
                    });
            }
            else
            {
                _progressView.Visibility = !show ? ViewStates.Gone : ViewStates.Visible;
                _loginFormView.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            }
        }
    }
}