using Android.App;
using Android.OS;
using Android.Widget;

namespace Ks.Dust.Android.DataPresenter
{
    [Activity(Label = "Ks.Dust.Android.DataPresenter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var btn = FindViewById(Resource.Id.btnHello);
            btn.Click += (sender, args) =>
            {
                Toast.MakeText(this, "Hello World!", ToastLength.Short).Show();
            };
        }
    }
}

