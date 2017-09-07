using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.Utils;
using Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using Newtonsoft.Json;
using MikePhil.Charting.Data;
using MikePhil.Charting.Interfaces.Datasets;
using MikePhil.Charting.Util;
using DividerItemDecoration = Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh.DividerItemDecoration;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class DeviceHistoryDataFragment : Fragment, IOnCheckMoreContentListener, IOnLoadingListener
    {
        private string _currentDeviceId;

        private HistoryDataType _dataType;

        private readonly List<DeviceHistoryData> _deviceHistoryDatas = new List<DeviceHistoryData>();

        [BindView(Resource.Id.historydataRefreshLayout)]
        private SuperRefreshLayout _magicRefreshLayout;

        private RecyclerView.LayoutManager _layoutManager;

        //private IParcelable _layoutManagerState;

        private DeviceHistoryDataAdapter _adapter;

        [BindView(Resource.Id.lineChart)]
        protected LineChart LineChart;

        private readonly Activity _ownerActivity;

        public DeviceHistoryDataFragment(Activity activity)
        {
            _ownerActivity = activity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _currentDeviceId = Arguments.GetString(ActivityConts.NameDeviceId);
            _dataType = (HistoryDataType)Arguments.GetInt(ActivityConts.NameDeviceHistoryDataType);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_device_historydata, container, false);
            Cheeseknife.Bind(this, view);
            InitView();
            GetData();
            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            _magicRefreshLayout.StopLoading();
            _magicRefreshLayout.OnLoadingListener = null;
            _magicRefreshLayout.OnCheckMoreContentListener = null;
            //_layoutManagerState = _layoutManager.OnSaveInstanceState();
            _magicRefreshLayout = null;
            _layoutManager = null;
            if (_adapter == null) return;
            _adapter.AdapterData = null;
            _adapter = null;
        }

        private void InitView()
        {
            _adapter = new DeviceHistoryDataAdapter(Activity)
            {
                AdapterData = _deviceHistoryDatas
            };

            _layoutManager = new LinearLayoutManager(Activity.ApplicationContext);

            _magicRefreshLayout.SetProgressViewEndTarget(false, Resources.GetDimensionPixelSize(Resource.Dimension.status_bar_height) + BaseUtils.Dip2Px(Activity, 37));
            _magicRefreshLayout.RecyclerView.HasFixedSize = true;
            _magicRefreshLayout.SetLayoutManager(_layoutManager);
            _magicRefreshLayout.RecyclerView.AddItemDecoration(new DividerItemDecoration(Activity, DividerItemDecoration.Vertical));
            _magicRefreshLayout.SetAdapter(_adapter);
            _magicRefreshLayout.OnLoadingListener = this;
            _magicRefreshLayout.OnCheckMoreContentListener = this;

            InitChartView();
        }

        private void InitChartView()
        {
            LineChart.SetTouchEnabled(true);
            LineChart.DragEnabled = true;
            LineChart.SetScaleEnabled(true);
            LineChart.ScaleYEnabled = false;
            LineChart.SetPinchZoom(true);
            LineChart.EnableScroll();

            LineChart.Description.Enabled = false;

            LineChart.XAxis.Enabled = false;
            LineChart.AxisLeft.Enabled = false;
            LineChart.AxisRight.Enabled = false;
        }

        private void GetData()
        {
            var handler = new AndroidHttpResponseHandler(Activity);
            handler.OnResponse += args =>
            {
                _ownerActivity.RunOnUiThread(() =>
                {
                    _magicRefreshLayout?.StopLoading();
                    var historyData = JsonConvert.DeserializeObject<List<DeviceHistoryData>>(args.Response);
                    _deviceHistoryDatas.Clear();
                    _deviceHistoryDatas.AddRange(historyData);
                    _adapter.NotifyDataSetChanged();
                    SetLineChartData();
                });
            };
            ApiManager.GetDeviceHistoryData(_currentDeviceId, $"{(int)_dataType}", AuthticationManager.Instance.AccessToken, handler);
        }

        private void SetLineChartData()
        {
            LineChart.Clear();
            var yVals1 = new List<Entry>();
            for (var i = 0; i < _deviceHistoryDatas.Count; i++)
            {
                var data = _deviceHistoryDatas[i];
                yVals1.Add(new Entry(i, (float)data.tsp));
            }

            LineDataSet set1;

            if (LineChart.LineData != null && LineChart.LineData.DataSetCount > 0)
            {
                set1 = (LineDataSet)LineChart.LineData.GetDataSetByIndex(0);
                set1.Values = yVals1;

                LineChart.LineData.NotifyDataChanged();
                LineChart.NotifyDataSetChanged();
            }
            else
            {
                set1 = new LineDataSet(yVals1, string.Empty);
                set1.AxisDependency = YAxis.AxisDependency.Left;
                set1.Color = ColorTemplate.HoloBlue;
                set1.SetCircleColor(Color.White);
                set1.LineWidth = 4f;
                set1.CircleRadius = 3f;
                set1.FillAlpha = 65;

                set1.SetDrawValues(_deviceHistoryDatas.Count != 0);

                set1.FillColor = ColorTemplate.HoloBlue;
                set1.HighLightColor = Color.Rgb(244, 117, 117);
                set1.SetDrawCircleHole(false);

                var dataSets = new List<ILineDataSet> {set1};

                var data = new LineData(dataSets);
                data.SetValueTextColor(Color.White);
                data.SetValueTextSize(12f);

                LineChart.Data = data;
                LineChart.Invalidate();
            }
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
    }
}