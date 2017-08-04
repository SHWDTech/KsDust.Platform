using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh;
using Android.OS;
using Android.Views;
using ApplicationConcept;
using Ks.Dust.AndroidDataPresenter.Xamarin.activity;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.Utils;
using Newtonsoft.Json;
using DividerItemDecoration = Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh.DividerItemDecoration;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class StatisticsFragment : Fragment, IOnStatisticsRecyclerItemClickListener, IOnCheckMoreContentListener, IOnLoadingListener
    {
        public const string StatisticsDataType = "Statistics_data_type";

        public const string StatisticsProjectType = "Statistics_project_type";

        private string _dataType = string.Empty;

        private string _projectType = string.Empty;

        private readonly List<DistrictGeneralInfo> _districtGeneralInfos = new List<DistrictGeneralInfo>();

        private StatisticsRecyclerAdapter _statisticsRecyclerAdapter;

        private StatisticsBarChartAdapter _statisticsBarChartAdapter;

        private RecyclerView.LayoutManager _layoutManager;

        protected bool IsOrderByTsgAvgAsc;

        [BindView(Resource.Id.recyclerView)]
        protected RecyclerView BarChartChartRecyclerView;

        [BindView(Resource.Id.statisticsRefreshLayout)] private SuperRefreshLayout _magicRefreshLayout;

        private readonly Activity _activity;

        private double _maxTsp;

        [OnClick(Resource.Id.districtavg_tspavg_tv)]
        protected void TspClick(object sender, EventArgs args)
        {
            if (IsOrderByTsgAvgAsc)
            {
                _districtGeneralInfos.Sort((x, y) => x.tspAvg.CompareTo(y.tspAvg));
                IsOrderByTsgAvgAsc = false;
            }
            else
            {
                _districtGeneralInfos.Sort((x, y) => y.tspAvg.CompareTo(x.tspAvg));
                IsOrderByTsgAvgAsc = true;
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
            BarChartChartRecyclerView.SetLayoutManager(layoutManager);
            var dividerItemDecoration = new DividerItemDecoration(Activity, DividerItemDecoration.Horizontal, true)
            {
                Width = 30
            };
            BarChartChartRecyclerView.AddItemDecoration(dividerItemDecoration);
            BarChartChartRecyclerView.SetAdapter(_statisticsBarChartAdapter);

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
                    var avgs = JsonConvert.DeserializeObject<List<DistrictGeneralInfo>>(args.Response);
                    _districtGeneralInfos.AddRange(avgs);

                    GetMaxTsp();
                    _statisticsBarChartAdapter.MaxValue = _maxTsp;
                    _statisticsBarChartAdapter.NotifyDataSetChanged();

                    _statisticsRecyclerAdapter.NotifyDataSetChanged();
                    _magicRefreshLayout?.StopLoading();
                });
            };
            ApiManager.GetDistrictAvg(_projectType, _dataType, AuthticationManager.Instance.AccessToken, handler);
        }

        public void OnStatisticsItemClick(int position)
        {
            var detail = _districtGeneralInfos[position];
            var intent = new Intent(Activity, typeof(DistrictDetailActivity));
            var bundle = new Bundle();
            bundle.PutString(DistrictDetailActivity.DistrictdetailProjecttype, _projectType);
            bundle.PutString(DistrictDetailActivity.DistrictdetailDistrictname, detail.name);
            bundle.PutString(DistrictDetailActivity.DistrictdetailDistrictid, detail.id);
            intent.PutExtras(bundle);
            StartActivity(intent);
        }

        public bool CanContentLoadMore()
        {
            return false;
        }

        public void OnRefresh()
        {
            GetData();
        }

        public void OnLoadMore()
        {
            
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