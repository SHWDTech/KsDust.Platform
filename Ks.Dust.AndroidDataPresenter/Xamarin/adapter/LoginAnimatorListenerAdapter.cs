using Android.Animation;
using Android.Views;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    class LoginAnimatorListenerAdapter : AnimatorListenerAdapter
    {
        public bool IsShow { get; set; }

        public View LoginView { get; set; }

        public override void OnAnimationEnd(Animator animation)
        {
            LoginView.Visibility = IsShow ? ViewStates.Gone : ViewStates.Visible;
            Dispose();
        }
    }
}