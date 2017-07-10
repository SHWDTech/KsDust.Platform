using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
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
            SetContentView(Resource.Layout.activity_statistics);
            Cheeseknife.Bind(this);

            var bundle = Intent.Extras;
            _projectType = bundle.GetString(NameStatisticsType);
            _elementType = bundle.GetString(NameStatisticsElementType);
            _elementId = bundle.GetString(NameStatisticsElementId);
            _isFromSearch = bundle.GetBoolean(NameStatisticsFromSearch);
            _name = bundle.GetString(NameStatisticsName);
            _titleTextView.Text = _name;

            _fragmentManager = FragmentManager;

            InitFragment();
        }

        private Fragment CreateFragment(string dataType)
        {
            Fragment newFragment;
            if (_isFromSearch)
            {
                newFragment = new SearchStatisticsFragment(this);
                var bundle = new Bundle();
                bundle.PutString(SearchStatisticsFragment.StatisticsDataType, dataType);
                bundle.PutString(SearchStatisticsFragment.StatisticsElementid, _elementId);
                bundle.PutString(SearchStatisticsFragment.StatisticsElementtype, _elementType);
                newFragment.Arguments = bundle;
            }
            else
            {
                newFragment = new StatisticsFragment(this);
                var bundle = new Bundle();
                bundle.PutString(StatisticsFragment.StatisticsDataType, dataType);
                bundle.PutString(StatisticsFragment.StatisticsProjectType, _projectType);
                newFragment.Arguments = bundle;
            }

            return newFragment;
        }

        private Fragment CreateFifteenMinFragment() => CreateFragment($"{ActivityConts.DataTypeFifteenMin}");

        private Fragment CreateHourFragment() => CreateFragment($"{ActivityConts.DataTypeHour}");

        private Fragment CreateDayFragment() => CreateFragment($"{ActivityConts.DataTypeDay}");

        private Fragment CreateMonthFragment() => CreateFragment($"{ActivityConts.DataTypeMonth}");

        [OnClick(Resource.Id.min15)]
        public void ClickFifteenMin(object sender, EventArgs args)
        {
            if (_currentPosition == _positionFifteenMin) return;

            _currentPosition = _positionFifteenMin;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_fifteenMinFragment == null)
            {
                _fifteenMinFragment = CreateFifteenMinFragment();
                transaction.Add(Resource.Id.contentLayout, _fifteenMinFragment);
            }
            else
            {
                transaction.Show(_fifteenMinFragment);
            }
            transaction.Commit();
        }

        [OnClick(Resource.Id.hour)]
        public void ClickHour(object sender, EventArgs args)
        {
            if (_currentPosition == _positionHour) return;

            _currentPosition = _positionHour;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_hourFragment == null)
            {
                _hourFragment = CreateHourFragment();
                transaction.Add(Resource.Id.contentLayout, _hourFragment);
            }
            else
            {
                transaction.Show(_hourFragment);
            }
            transaction.Commit();
        }

        [OnClick(Resource.Id.day)]
        public void ClickDay(object sender, EventArgs args)
        {
            if (_currentPosition == _positionDay) return;

            _currentPosition = _positionDay;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_dayFragment == null)
            {
                _dayFragment = CreateDayFragment();
                transaction.Add(Resource.Id.contentLayout, _dayFragment);
            }
            else
            {
                transaction.Show(_dayFragment);
            }
            transaction.Commit();
        }

        [OnClick(Resource.Id.month)]
        public void ClickMonth(object sender, EventArgs args)
        {
            if (_currentPosition == _positionMonth) return;

            _currentPosition = _positionMonth;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_monthFragment == null)
            {
                _monthFragment = CreateMonthFragment();
                transaction.Add(Resource.Id.contentLayout, _monthFragment);
            }
            else
            {
                transaction.Show(_monthFragment);
            }
            transaction.Commit();
        }

        [OnClick(Resource.Id.back)]
        private void Back(object sender, EventArgs args)
        {
            Finish();
        }

        private void InitFragment()
        {
            _fifteenMinFragment = CreateFifteenMinFragment();
            var transaction = _fragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.contentLayout, _fifteenMinFragment).Commit();
        }

        private void HideFragments(FragmentTransaction transaction)
        {
            if (_fifteenMinFragment != null)
            {
                transaction.Hide(_fifteenMinFragment);
            }
            if (_hourFragment != null)
            {
                transaction.Hide(_hourFragment);
            }
            if (_dayFragment != null)
            {
                transaction.Hide(_dayFragment);
            }
            if (_monthFragment != null)
            {
                transaction.Hide(_monthFragment);
            }
        }

        private void SetIcon()
        {
            switch (_currentPosition)
            {
                case _positionFifteenMin:
                    _fifteenMinTextView.SetTextColor(Color.White);
                    _hourTextView.SetTextColor(Color.Black);
                    _dayTextView.SetTextColor(Color.Black);
                    _monthTextView.SetTextColor(Color.Black);
                    break;
                case _positionHour:
                    _fifteenMinTextView.SetTextColor(Color.Black);
                    _hourTextView.SetTextColor(Color.White);
                    _dayTextView.SetTextColor(Color.Black);
                    _monthTextView.SetTextColor(Color.Black);
                    break;
                case _positionDay:
                    _fifteenMinTextView.SetTextColor(Color.Black);
                    _hourTextView.SetTextColor(Color.Black);
                    _dayTextView.SetTextColor(Color.White);
                    _monthTextView.SetTextColor(Color.Black);
                    break;
                case _positionMonth:
                    _fifteenMinTextView.SetTextColor(Color.Black);
                    _hourTextView.SetTextColor(Color.Black);
                    _dayTextView.SetTextColor(Color.Black);
                    _monthTextView.SetTextColor(Color.White);
                    break;
            }
        }
    }
}