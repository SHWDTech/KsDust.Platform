using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.fragment;
using Android.Graphics;
using ApplicationConcept;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(DeviceDetailActivity))]
    public class DeviceDetailActivity : KsDustBaseActivity
    {
        private DeviceCurrentDataFragment _currentDataFragment;

        private DeviceHistoryDataFragment _fifteenMinFragment, _hourFragment, _dayFragment, _monthFragment;

        [BindView(Resource.Id.device_name)] protected TextView TitleTextView;

        [BindView(Resource.Id.back)] protected View BackView;

        [BindView(Resource.Id.current)] protected TextView CurrentTextView;

        [BindView(Resource.Id.min15)] protected TextView FifteenMinTextView;

        [BindView(Resource.Id.hour)] protected TextView HourTextView;

        [BindView(Resource.Id.day)] protected TextView DayTextView;

        [BindView(Resource.Id.month)] protected TextView MonthTextView;

        private string _deviceId;

        private string _deviceName;

        private int _currentDataType;

        private FragmentManager _fragmentManager;

        public const int CurrentDataType = 0;

        public const int DataTypeFifteenMin = 1;

        public const int DataTypeHour = 2;

        public const int DataTypeDay = 3;

        public const int DataTypeMonth = 4;

        [OnClick(Resource.Id.current)]
        public void SwitchToCurrent(object sender, EventArgs args)
        {
            if (_currentDataType == CurrentDataType) return;
            _currentDataType = CurrentDataType;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_currentDataFragment == null)
            {
                _currentDataFragment = new DeviceCurrentDataFragment(this);
                var bundle = new Bundle();
                bundle.PutString(ActivityConts.NameDeviceId, _deviceId);
                _currentDataFragment.Arguments = bundle;
                transaction.Add(Resource.Id.contentLayout, _currentDataFragment);
            }
            else
            {
                transaction.Show(_currentDataFragment);
            }

            transaction.Commit();
        }

        [OnClick(Resource.Id.min15)]
        public void SwitchToFifteen(object sender, EventArgs args)
        {
            if (_currentDataType == DataTypeFifteenMin) return;
            _currentDataType = DataTypeFifteenMin;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_fifteenMinFragment == null)
            {
                _fifteenMinFragment = new DeviceHistoryDataFragment(this);
                var bundle = new Bundle();
                bundle.PutString(ActivityConts.NameDeviceId, _deviceId);
                bundle.PutInt(ActivityConts.NameDeviceHistoryDataType, (int) HistoryDataType.FifteenAvg);
                _fifteenMinFragment.Arguments = bundle;
                transaction.Add(Resource.Id.contentLayout, _fifteenMinFragment);
            }
            else
            {
                transaction.Show(_fifteenMinFragment);
            }

            transaction.Commit();
        }

        [OnClick(Resource.Id.hour)]
        public void SwitchToHour(object sender, EventArgs args)
        {
            if (_currentDataType == DataTypeHour) return;
            _currentDataType = DataTypeHour;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_hourFragment == null)
            {
                _hourFragment = new DeviceHistoryDataFragment(this);
                var bundle = new Bundle();
                bundle.PutString(ActivityConts.NameDeviceId, _deviceId);
                bundle.PutInt(ActivityConts.NameDeviceHistoryDataType, (int)HistoryDataType.HourAvg);
                _hourFragment.Arguments = bundle;
                transaction.Add(Resource.Id.contentLayout, _hourFragment);
            }
            else
            {
                transaction.Show(_hourFragment);
            }

            transaction.Commit();
        }

        [OnClick(Resource.Id.day)]
        public void SwitchToDay(object sender, EventArgs args)
        {
            if (_currentDataType == DataTypeDay) return;
            _currentDataType = DataTypeDay;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_dayFragment == null)
            {
                _dayFragment = new DeviceHistoryDataFragment(this);
                var bundle = new Bundle();
                bundle.PutString(ActivityConts.NameDeviceId, _deviceId);
                bundle.PutInt(ActivityConts.NameDeviceHistoryDataType, (int)HistoryDataType.DayAvg);
                _dayFragment.Arguments = bundle;
                transaction.Add(Resource.Id.contentLayout, _dayFragment);
            }
            else
            {
                transaction.Show(_dayFragment);
            }

            transaction.Commit();
        }

        [OnClick(Resource.Id.month)]
        public void SwitchToMonth(object sender, EventArgs args)
        {
            if (_currentDataType == DataTypeMonth) return;
            _currentDataType = DataTypeMonth;
            SetIcon();

            var transaction = _fragmentManager.BeginTransaction();
            HideFragments(transaction);
            if (_monthFragment == null)
            {
                _monthFragment = new DeviceHistoryDataFragment(this);
                var bundle = new Bundle();
                bundle.PutString(ActivityConts.NameDeviceId, _deviceId);
                bundle.PutInt(ActivityConts.NameDeviceHistoryDataType, (int)HistoryDataType.MonthAvg);
                _monthFragment.Arguments = bundle;
                transaction.Add(Resource.Id.contentLayout, _monthFragment);
            }
            else
            {
                transaction.Show(_monthFragment);
            }

            transaction.Commit();
        }

        [OnClick(Resource.Id.back)]
        public void Back(object sender, EventArgs args)
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_device_detail);
            Cheeseknife.Bind(this);
            _fragmentManager = FragmentManager;

            var bundle = Intent.Extras;
            _deviceId = bundle.GetString(ActivityConts.NameDeviceId);
            _deviceName = bundle.GetString(ActivityConts.NameDeviceName);
            TitleTextView.Text = _deviceName;

            InitFragment();
        }

        private void InitFragment()
        {
            _currentDataFragment = new DeviceCurrentDataFragment(this);
            var bundle = new Bundle();
            bundle.PutString(ActivityConts.NameDeviceId, _deviceId);
            _currentDataFragment.Arguments = bundle;
            var transaction = _fragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.contentLayout, _currentDataFragment).Commit();
        }

        private void HideFragments(FragmentTransaction transaction)
        {
            if (_currentDataFragment != null)
            {
                transaction.Hide(_currentDataFragment);
            }
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
            switch (_currentDataType)
            {
                case CurrentDataType:
                    CurrentTextView.SetTextColor(Color.White);
                    FifteenMinTextView.SetTextColor(Color.Black);
                    HourTextView.SetTextColor(Color.Black);
                    DayTextView.SetTextColor(Color.Black);
                    MonthTextView.SetTextColor(Color.Black);
                    break;
                case DataTypeFifteenMin:
                    CurrentTextView.SetTextColor(Color.Black);
                    FifteenMinTextView.SetTextColor(Color.White);
                    HourTextView.SetTextColor(Color.Black);
                    DayTextView.SetTextColor(Color.Black);
                    MonthTextView.SetTextColor(Color.Black);
                    break;
                case DataTypeHour:
                    CurrentTextView.SetTextColor(Color.Black);
                    FifteenMinTextView.SetTextColor(Color.Black);
                    HourTextView.SetTextColor(Color.White);
                    DayTextView.SetTextColor(Color.Black);
                    MonthTextView.SetTextColor(Color.Black);
                    break;
                case DataTypeDay:
                    CurrentTextView.SetTextColor(Color.Black);
                    FifteenMinTextView.SetTextColor(Color.Black);
                    HourTextView.SetTextColor(Color.Black);
                    DayTextView.SetTextColor(Color.White);
                    MonthTextView.SetTextColor(Color.Black);
                    break;
                case DataTypeMonth:
                    CurrentTextView.SetTextColor(Color.Black);
                    FifteenMinTextView.SetTextColor(Color.Black);
                    HourTextView.SetTextColor(Color.Black);
                    DayTextView.SetTextColor(Color.Black);
                    MonthTextView.SetTextColor(Color.White);
                    break;
            }
        }
    }
}