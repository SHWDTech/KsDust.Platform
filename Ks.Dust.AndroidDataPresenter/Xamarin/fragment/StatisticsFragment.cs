using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Support.V7.Widget;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh;
using Android.OS;
using Android.Views;
using Ks.Dust.AndroidDataPresenter.Xamarin.Utils;
using DividerItemDecoration = Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh.DividerItemDecoration;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class StatisticsFragment : Fragment, IOnStatisticsRecyclerItemClickListener, IOnCheckMoreContentListener, IOnLoadingListener
    {
        public const string StatisticsDataType = "Statistics_data_type";

        public const string StatisticsProjectType = "Statistics_project_type";

        private string _dataType = string.Empty;

        private string _projectType = string.Empty;

        private List<DistrictGeneralInfo> _districtGeneralInfos = new List<DistrictGeneralInfo>();

        private StatisticsRecyclerAdapter _statisticsRecyclerAdapter;

        private StatisticsBarChartAdapter _statisticsBarChartAdapter;

        private RecyclerView.LayoutManager _layoutManager;

        private bool _isOrderBYTsgAvgAsc = false;

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

        public StatisticsFragment(Activity activity)
        {
            _activity = activity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var bundle = Arguments;
            _dataType = bundle.GetString(StatisticsDataType);
            _projectType = bundle.GetString(StatisticsProjectType);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_statistics, container, false);
            Cheeseknife.Bind(this, view);
            _statisticsBarChartAdapter = new StatisticsBarChartAdapter(Activity, _districtGeneralInfos);
            _statisticsRecyclerAdapter =
                new StatisticsRecyclerAdapter(Activity, _districtGeneralInfos) {OnItemClickListener = this};

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

        private void GetData()
        {
            
        }

        public void OnStatisticsItemClick(int position)
        {
            throw new NotImplementedException();
        }

        public bool CanContentLoadMore()
        {
            throw new NotImplementedException();
        }

        public void OnRefresh()
        {
            throw new NotImplementedException();
        }

        public void OnLoadMore()
        {
            throw new NotImplementedException();
        }
    }
}