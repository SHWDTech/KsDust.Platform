using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.Utils;
using Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh;
using Newtonsoft.Json;
using DividerItemDecoration = Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh.DividerItemDecoration;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(CascadeElementActivity))]
    public class CascadeElementActivity : KsDustBaseActivity, IOnCascadeElementItemListener, IOnLoadingListener, IOnCheckMoreContentListener
    {
        public const string CascadeElementId = "cascadeElementId";

        public const string CascadeElementLevel = "cascadeElementLevel";

        public const string CascadeElementName = "cascadeElementName";

        private string _cascadeElementName;

        private string _cascadeElementId;

        private ObjectType _cascadeElementLevel;

        [BindView(Resource.Id.refreshLayout)]
        private SuperRefreshLayout _magicRefreshLayout;

        [BindView(Resource.Id.title)]
        protected TextView TitleView;

        private RecyclerView.LayoutManager _layoutManager;

        private readonly List<CascadeElement> _cascadeElements = new List<CascadeElement>();

        private CascadeElementAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_cascade_element);
            Cheeseknife.Bind(this);
            var bundle = Intent.Extras;
            _cascadeElementId = bundle.GetString(CascadeElementId);
            _cascadeElementName = bundle.GetString(CascadeElementName);
            _cascadeElementLevel = (ObjectType)bundle.GetInt(CascadeElementLevel);
            TitleView.Text = _cascadeElementLevel == ObjectType.WholeCity ? "全部区县" : _cascadeElementName;

            _adapter = new CascadeElementAdapter(this, _cascadeElements)
            {
                OnCascadeElementItemListener = this
            };
            _layoutManager = new LinearLayoutManager(ApplicationContext);

            _magicRefreshLayout.SetProgressViewEndTarget(false, Resources.GetDimensionPixelSize(Resource.Dimension.status_bar_height) + BaseUtils.Dip2Px(this, 37));
            _magicRefreshLayout.RecyclerView.HasFixedSize = true;
            _magicRefreshLayout.SetLayoutManager(_layoutManager);
            _magicRefreshLayout.RecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
            _magicRefreshLayout.SetAdapter(_adapter);
            _magicRefreshLayout.OnLoadingListener = this;
            _magicRefreshLayout.OnCheckMoreContentListener = this;

            _magicRefreshLayout.StartRefresh();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _magicRefreshLayout.StopLoading();
            _magicRefreshLayout.OnLoadingListener = null;
            _magicRefreshLayout.OnCheckMoreContentListener = null;
            _magicRefreshLayout = null;
            _layoutManager = null;

            if (_adapter != null)
            {
                _adapter.OnCascadeElementItemListener = null;
                _adapter.AdapterData = null;
                _adapter = null;
            }
        }

        [OnClick(Resource.Id.back)]
        protected void Back(object sender, EventArgs args)
        {
            Finish();
        }

        private void GetData()
        {
            var handler = new HttpResponseHandler();
            handler.OnResponse += (args) =>
            {
                RunOnUiThread(() =>
                {
                    _magicRefreshLayout?.StopLoading();

                    var elements = JsonConvert.DeserializeObject<List<CascadeElement>>(args.Response);
                    _cascadeElements.Clear();
                    _cascadeElements.AddRange(elements);
                    _adapter.NotifyDataSetChanged();
                });
            };
            ApiManager.GetCascadeElement($"{(int)_cascadeElementLevel}", _cascadeElementId, AuthticationManager.Instance.AccessToken, handler);
        }

        public void OnCascadeElementItemClick(int position)
        {
            var element = _cascadeElements[position];
            if (_cascadeElementLevel == ObjectType.Project)
            {
                var intent = new Intent(this, typeof(DeviceDetailActivity));
                var bundle = new Bundle();
                bundle.PutString(ActivityConts.NameDeviceId, element.cascadeElementId);
                bundle.PutString(ActivityConts.NameDeviceName, element.cascadeElementName);
                intent.PutExtras(bundle);
                StartActivity(intent);
            }
            else
            {
                var intent = new Intent(this, typeof(CascadeElementActivity));
                var bundle = new Bundle();
                bundle.PutString(CascadeElementId, element.cascadeElementId);
                bundle.PutString(CascadeElementName, element.cascadeElementName);
                bundle.PutInt(CascadeElementLevel, element.cascadeElementLevel);
                intent.PutExtras(bundle);
                StartActivity(intent);
            }
        }

        public void OnRefresh()
        {
            GetData();
        }

        public void OnLoadMore()
        {
            
        }

        public bool CanContentLoadMore()
        {
            return false;
        }
    }
}