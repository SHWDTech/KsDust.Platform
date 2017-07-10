using Android.App;
using Android.OS;
using Android.Views;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = "ReferenceStandardActivity")]
    public class ReferenceStandardActivity : KsDustBaseActivity, View.IOnClickListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_reference);
            FindViewById(Resource.Id.back).SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            Finish();
        }
    }
}