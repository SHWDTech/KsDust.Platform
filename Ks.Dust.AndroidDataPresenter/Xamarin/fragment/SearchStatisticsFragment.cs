using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.Utils;
using Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh;
using Newtonsoft.Json;
using DividerItemDecoration = Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh.DividerItemDecoration;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class SearchStatisticsFragment : Fragment, IOnCheckMoreContentListener, IOnLoadingListener
    {
        public const string StatisticsDataType = "Statistics_data_type";

        public const string StatisticsElementtype = "statistics_elementtype";

        public const string StatisticsElementid = "statistics_elementid";

        private string _dataType = string.Empty;

        private string _elementType = string.Empty;

        private string _elementId = string.Empty;

        private List<DistrictGeneralInfo> _districtGeneralInfos = new List<DistrictGeneralInfo>();

        private StatisticsRecyclerAdapter _statisticsRecyclerAdapter;

        private StatisticsBarChartAdapter _statisticsBarChartAdapter;

        private RecyclerView.LayoutManager _layoutManager;

        private bool _isOrderBYTsgAvgAsc = false;

        private double _maxTsp;

        [BindView(Resource.Id.recyclerView)] private RecyclerView _barChartChartRecyclerView;

        [BindView(Resource.Id.statisticsRefreshLayout)] private SuperRefreshLayout _magicRefreshLayout;

        private readonly Activity _activity;

        [OnClick(Resource.Id.districtavg_tspavg_tv)]
        private void TspClick(object sender, EventArgs args)
        {
            if (_isOrderBYTsgAvgAsc)
            {
                _districtGeneralInfos = _districtGeneralInfos.OrderByDescending(info => info.tspAvg).ToList();
                _isOrderBYTsgAvgAsc = false;
            }
            else
            {
                _districtGeneralInfos = _districtGeneralInfos.OrderBy(info => info.tspAvg).ToList();
                _isOrderBYTsgAvgAsc = true;
            }
            _statisticsRecyclerAdapter.NotifyDataSetChanged();
        }

        public SearchStatisticsFragment(Activity activity)
        {
            _activity = activity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var bundle = Arguments;
            _dataType = bundle.GetString(StatisticsDataType);
            _elementType = bundle.GetString(StatisticsElementtype);
            _elementId = bundle.GetString(StatisticsElementid);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_statistics, container, false);
            Cheeseknife.Bind(this, view);
            _statisticsBarChartAdapter = new StatisticsBarChartAdapter(Activity, _districtGeneralInfos);
            _statisticsRecyclerAdapter = new StatisticsRecyclerAdapter(Activity, _districtGeneralInfos);

            _layoutManager = new LinearLayoutManager(Activity.ApplicationContext);
            _magicRefreshLayout.SetProgressViewEndTarget(false,
                Resources.GetDimensionPixelSize(Resource.Dimension.status_bar_height) + BaseUtils.Dip2Px(Activity, 37));
            _magicRefreshLayout.RecyclerView.HasFixedSize = true;
            _magicRefreshLayout.SetLayoutManager(_layoutManager);
            _magicRefreshLayout.RecyclerView.AddItemDecoration(new DividerItemDecoration(Activity, DividerItemDecoration.Vertical));
            _magicRefreshLayout.SetAdapter(_statisticsRecyclerAdapter);
            _magicRefreshLayout.OnLoadingListener = this;
            _magicRefreshLayout.OnCheckMoreContentListener = this;

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            _barChartChartRecyclerView.SetLayoutManager(layoutManager);
            var dividerItemDecoration = new DividerItemDecoration(Activity, DividerItemDecoration.Horizontal, true)
            {
                Width = 30
            };
            _barChartChartRecyclerView.AddItemDecoration(dividerItemDecoration);
            _barChartChartRecyclerView.SetAdapter(_statisticsBarChartAdapter);

            GetData();
            return view;
        }

        private void GetMaxTsp()
        {
            var currentMax = _districtGeneralInfos.Max(info => info.tspAvg);
            if (currentMax > _maxTsp)
            {
                _maxTsp = currentMax;
            }
        }

        private void GetData()
        {
            _districtGeneralInfos.Clear();
            _maxTsp = 0;

            var handler = new HttpResponseHandler();
            handler.OnResponse += args =>
            {
                _activity.RunOnUiThread(() =>
                {
                    var districtInfos = JsonConvert.DeserializeObject<List<DistrictGeneralInfo>>(args.Response);
                    _districtGeneralInfos.AddRange(districtInfos);

                    GetMaxTsp();
                    _statisticsBarChartAdapter.MaxValue = _maxTsp;
                    _statisticsBarChartAdapter.NotifyDataSetChanged();

                    _statisticsRecyclerAdapter.NotifyDataSetChanged();
                    _magicRefreshLayout?.StopLoading();
                });
            };

            ApiManager.GetStatisticsDetail(_dataType, _elementType, _elementId, AuthticationManager.Instance.AccessToken, handler);
        }

        public void OnLoadMore()
        {
            
        }

        public void OnRefresh()
        {
            GetData();
        }

        public bool CanContentLoadMore()
        {
            return false;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            if (_magicRefreshLayout != null)
            {
                _magicRefreshLayout.StopLoading();
                _magicRefreshLayout.OnLoadingListener = null;
                _magicRefreshLayout.OnCheckMoreContentListener = null;
            }
            _magicRefreshLayout = null;
            _layoutManager = null;
            if (_statisticsRecyclerAdapter != null)
            {
                _statisticsRecyclerAdapter.OnItemClickListener = null;
                _statisticsRecyclerAdapter.AdapterData = null;
                _statisticsRecyclerAdapter = null;
            }
        }
    }
}