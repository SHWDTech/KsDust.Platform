using Android.App;
using Android.OS;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(StatisticsActivity))]
    public class StatisticsActivity : KsDustBaseActivity
    {
        public const string NameStatisticsType = "Statistics_type";

        public const string NameStatisticsName = "Statistics_name";

        public const string NameStatisticsElementType = "Statistics_elementType";

        public const string NameStatisticsElementId = "Statistics_ellementId";

        public const string NameStatisticsFromSearch = "Statistics_fromSearch";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}