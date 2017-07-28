using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.fragment;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.Utils;
using Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh;
using Newtonsoft.Json;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(DistrictDetailActivity))]
    public class DistrictDetailActivity : KsDustBaseActivity, IOnCheckMoreContentListener, IOnLoadingListener, 
        View.IOnClickListener, IOnDistrictDetailItemClickListener
    {
        public const string DistrictdetailProjecttype = "projectType";

        public const string DistrictdetailDistrictid = "districtId";

        public const string DistrictdetailDistrictname = "districtName";

        private string _projectType;

        private string _districtId;

        private string _districtName;

        private TextView _titleView;

        private SuperRefreshLayout _magicRefreshLayout;

        private RecyclerView.LayoutManager _layoutManager;
        // RecyclerView State
        private IParcelable _layoutManagerState;

        private List<DistrictDetail> _districtDetails = new List<DistrictDetail>();

        private DistrictDetailAdapter _adapter;

        private View _districtdetailTsplayout;

        /** 默认是按照 浓度从小到大排列 */
        private bool _isOrderByTsgAvgAsc = false;

        private View _mapImageView;

        [OnClick(Resource.Id.back)]
        protected void Back(object sender, EventArgs args)
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_districtdetail);
            Cheeseknife.Bind(this);
            InitView();
            InitData();
        }

        private void InitView()
        {
            _adapter = new DistrictDetailAdapter(this)
            {
                OnDistrictDetailItemClickListener = this,
                AdapterData = _districtDetails
            };
            _districtdetailTsplayout = FindViewById(Resource.Id.districtdetail_tsp_layout);
            _districtdetailTsplayout.SetOnClickListener(this);
            _mapImageView = FindViewById(Resource.Id.title_map);
            _mapImageView.SetOnClickListener(this);

            _titleView = (TextView) FindViewById(Resource.Id.title);
            _magicRefreshLayout = (SuperRefreshLayout) FindViewById(Resource.Id.districtDetailRefreshLayout);
            _layoutManager = new LinearLayoutManager(ApplicationContext);

            _magicRefreshLayout.SetProgressViewEndTarget(false, Resources.GetDimensionPixelSize(Resource.Dimension.status_bar_height)
                                                                + BaseUtils.Dip2Px(this, 37));
            _magicRefreshLayout.RecyclerView.HasFixedSize = true;
            _magicRefreshLayout.SetLayoutManager(_layoutManager);
            _magicRefreshLayout.RecyclerView.AddItemDecoration(new view.refresh.DividerItemDecoration(this, view.refresh.DividerItemDecoration.Vertical));
            _magicRefreshLayout.SetAdapter(_adapter);
            _magicRefreshLayout.OnLoadingListener = this;
            _magicRefreshLayout.OnCheckMoreContentListener = this;

        }

        private void InitData()
        {
            var bundle = Intent.Extras;
            _projectType = bundle.GetString(DistrictdetailProjecttype, string.Empty);
            _districtId = bundle.GetString(DistrictdetailDistrictid);
            _districtName = bundle.GetString(DistrictdetailDistrictname);
            _titleView.Text = _districtName.Trim();

            if (_districtDetails.Count == 0)
            {
                _magicRefreshLayout.StartRefresh();
            }
            else
            {
                _magicRefreshLayout.StopLoading();

                if (_layoutManagerState != null)
                {
                    _layoutManager.OnRestoreInstanceState(_layoutManagerState);
                    _layoutManagerState = null;
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _magicRefreshLayout.StopLoading();
            _magicRefreshLayout.OnLoadingListener = (null);
            _magicRefreshLayout.OnCheckMoreContentListener = (null);
            _layoutManagerState = _layoutManager.OnSaveInstanceState();
            _magicRefreshLayout = null;
            _layoutManager = null;

            if (_adapter != null)
            {
                _adapter.OnDistrictDetailItemClickListener = null;
                _adapter.AdapterData = null;
                _adapter = null;
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

        private void GetData()
        {
            var handler = new HttpResponseHandler();
            handler.OnResponse += args =>
            {
                RunOnUiThread(() =>
                {
                    _magicRefreshLayout?.StopLoading();

                    _districtDetails.Clear();
                    _districtDetails.AddRange(JsonConvert.DeserializeObject<List<DistrictDetail>>(args.Response));
                    _adapter.NotifyDataSetChanged();
                });
            };
            ApiManager.GetDistrictDetial(_projectType, _districtId, AuthticationManager.Instance.AccessToken, handler);
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.districtdetail_tsp_layout:
                    _districtDetails = _isOrderByTsgAvgAsc 
                        ? _districtDetails.OrderBy(d => d.tsp).ToList() 
                        : _districtDetails.OrderByDescending(d => d.tsp).ToList();
                    _adapter.NotifyDataSetChanged();
                    break;
                case Resource.Id.title_map:
                    var intent = new Intent(this, typeof(DistrictDetailMapActivity));
                    var bundle = new Bundle();
                    bundle.PutString(CategoryMapFragment.CategorymapProjecttype, _projectType);
                    bundle.PutString(CategoryMapFragment.CategorymapDistrictid, _districtId);
                    bundle.PutString(DistrictDetailMapActivity.DistrictdetailmapDistrictname, _districtName);
                    intent.PutExtras(bundle);
                    StartActivity(intent);
                    break;
            }
        }

        public void OnDistrictItemClick(int position)
        {
            var districtDetail = _districtDetails[position];
            var intent = new Intent(this, typeof(DeviceDetailActivity));
            var bundle = new Bundle();
            bundle.PutString(ActivityConts.NameDeviceId, districtDetail.id);
            bundle.PutString(ActivityConts.NameDeviceName, districtDetail.name);
            intent.PutExtras(bundle);
            StartActivity(intent);
        }
    }
}