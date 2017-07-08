using Android.App;
using Android.Content;
using Android.Support.V7.App;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(KsDustBaseActivity))]
    public class KsDustBaseActivity : AppCompatActivity
    {
        protected ISharedPreferences ActivityPreferences { get; set; }
    }
}