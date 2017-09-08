using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(KsDustBaseActivity))]
    public class KsDustBaseActivity : AppCompatActivity
    {
        public AuthticationManager AuthticationManager { get; protected set; }

        protected ISharedPreferences ActivityPreferences { get; set; }

        public KsDustBaseActivity()
        {
            AuthticationManager = AuthticationManager.Instance;
        }
    }
}