using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.fragment;

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

        private string _projectType = string.Empty;

        private string _elementType = string.Empty;

        private string _elementId = string.Empty;

        private bool _isFromSearch = false;

        private string _name = string.Empty;

        [BindView(Resource.Id.title)] private TextView _titleTextView;

        [BindView(Resource.Id.back)] private View _backView;

        [BindView(Resource.Id.min15)] private TextView _fifteenMinTextView;

        [BindView(Resource.Id.hour)] private TextView _hourTextView;

        [BindView(Resource.Id.day)] private TextView _dayTextView;

        [BindView(Resource.Id.month)] private TextView _monthTextView;

        private Fragment _fifteenMinFragment, _hourFragment, _dayFragment, _monthFragment;

        private int _currentPosition = 0;

        private FragmentManager _fragmentManager;

        public const int _positionFifteenMin = 0;

        public const int _positionHour = 1;

        public const int _positionDay = 2;

        public const int _positionMonth = 3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        private Fragment CreateFragment(string dataType)
        {
            Fragment fragment;
            if (_isFromSearch)
            {
                fragment = new SearchStatisticsFragment(this);
                var bundle = new Bundle();
                bundle.PutString(SearchStatisticsFragment.StatisticsDataType, dataType);
                bundle.PutString(SearchStatisticsFragment.StatisticsElementid, _elementId);
                bundle.PutString(SearchStatisticsFragment.StatisticsElementtype, _elementType);
                fragment.Arguments = bundle;
            }
            else
            {
                fragment = new StatisticsFragment();
                var bundle = new Bundle();
                bundle.PutString(StatisticsFragment.StatisticsDataType, dataType);
                bundle.PutString(StatisticsFragment.StatisticsProjectType, _projectType);
                fragment.Arguments = bundle;
            }

            return null;
        }
    }
}