using Android.App;
using Android.OS;
using CheeseBind;

namespace Ks.Dust.Android.DataPresenter.Resources.activity
{
    [Activity(Label = "Ks.Dust.Android.DataPresenter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Bind(this);
        }
    }
}

