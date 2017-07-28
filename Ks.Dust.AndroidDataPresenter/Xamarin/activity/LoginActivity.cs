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
        protected AutoCompleteTextView AccountView;

        [BindView(Resource.Id.password)]
        protected EditText PasswordView;

        [BindView(Resource.Id.login_progress)]
        protected View ProgressView;

        [BindView(Resource.Id.login_form)]
        protected View LoginFormView;

        private readonly AuthticationManager _authcicationManager = AuthticationManager.Instance;

        [OnClick(Resource.Id.sign_in_button)]
        protected void TryLogin(object sender, EventArgs e)
        {
            AccountView.SetError(string.Empty, null);
            PasswordView.SetError(string.Empty, null);

            var cancel = false;
            View focusView = null;
            if (string.IsNullOrWhiteSpace(PasswordView.Text))
            {
                PasswordView.SetError("账号不能为空", null);
                cancel = true;
                focusView = PasswordView;
            }
            if (string.IsNullOrWhiteSpace(AccountView.Text))
            {
                AccountView.SetError("账号不能为空", null);
                cancel = true;
                focusView = AccountView;
            }

            if (cancel)
            {
                focusView.RequestFocus();
            }
            else
            {
                ShowProgress(true);

                _authcicationManager.Login(AccountView.Text, PasswordView.Text);
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
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "登录成功", ToastLength.Short).Show();
                    using (var intent = new Intent(this, typeof(MainActivity)))
                    {
                        StartActivity(intent);
                    }
                    Finish();
                });
            }
        }

        private void ShowProgress(bool show)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
            {
                var shortAnimTime = Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);
                LoginFormView.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
                LoginFormView.Animate().SetDuration(shortAnimTime).Alpha(show ? 0 : 1)
                    .SetListener(new LoginAnimatorListenerAdapter
                    {
                        IsShow = show,
                        LoginView = LoginFormView
                    });

                ProgressView.Visibility = !show ? ViewStates.Gone : ViewStates.Visible;
                LoginFormView.Animate().SetDuration(shortAnimTime).Alpha(show ? 0 : 1)
                    .SetListener(new LoginAnimatorListenerAdapter
                    {
                        IsShow = !show,
                        LoginView = ProgressView
                    });
            }
            else
            {
                ProgressView.Visibility = !show ? ViewStates.Gone : ViewStates.Visible;
                LoginFormView.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            }
        }
    }
}