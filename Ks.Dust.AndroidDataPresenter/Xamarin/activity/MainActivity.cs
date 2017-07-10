using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.fragment;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.view;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = "Ks.Dust.AndroidDataPresenter", Icon = "@drawable/icon")]
    public class MainActivity : KsDustBaseActivity, View.IOnClickListener, NavigationView.IOnNavigationItemSelectedListener, IOnSearchClickListener
    {
        [BindView(Resource.Id.main_layout)]
        private DrawerLayout _drawer;

        [BindView(Resource.Id.goDrawer)]
        private View _indexView;

        [BindView(Resource.Id.home)]
        private ImageView _homeView;

        [BindView(Resource.Id.build)]
        private ImageView _buildView;

        [BindView(Resource.Id.municipal)]
        private ImageView _municipalView;

        [BindView(Resource.Id.mixing)]
        private ImageView _mixingView;

        [BindView(Resource.Id.title)]
        private TextView _cityTitleView;

        [BindView(Resource.Id.CascadeElement)]
        private View _cascadeElementView;

        [BindView(Resource.Id.Statistics)]
        private View _statisticsView;

        private MapViewType _currentMapViewType = MapViewType.Home;

        private CategoryMapFragment _homeFragment, _buildFragment, _municipalFragment, _mixingFragment;

        private List<string> _cityList = new List<string>();

        private PopupWindow _indexPopupWindow;

        [BindView(Resource.Id.search)]
        private View _searchView;

        private SearchDialog _searchDialog;

        private bool _isCheckByUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            try
            {
                Cheeseknife.Bind(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            InitView();
            InitFragment();
            _searchDialog = new SearchDialog(this);
            _searchDialog.SetOnClickListener(this);
        }

        private void InitView()
        {
            _indexView.SetOnClickListener(this);
            _homeView.SetOnClickListener(this);
            _buildView.SetOnClickListener(this);
            _municipalView.SetOnClickListener(this);
            _mixingView.SetOnClickListener(this);
            _cascadeElementView.SetOnClickListener(this);
            _statisticsView.SetOnClickListener(this);
            _cityTitleView.Text = "昆山";
            _searchView.SetOnClickListener(this);
            ((NavigationView)FindViewById(Resource.Id.nav_view)).SetNavigationItemSelectedListener(this);
        }

        private void InitFragment()
        {
            _homeFragment = new CategoryMapFragment();
            var bundle = new Bundle();
            _homeFragment.Arguments = bundle;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.contentLayout, _homeFragment).Commit();
        }

        public override void OnBackPressed()
        {
            if (_drawer.IsDrawerOpen(GravityCompat.Start))
            {
                _drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                using (var builder = new AlertDialog.Builder(this)
                )
                {
                    builder.SetMessage("确定退出吗？")
                                      .SetPositiveButton("退出", delegate
                                      {
                                          Finish();
                                      })
                                      .SetNegativeButton("取消", delegate { })
                                      .Create()
                                      .Show();
                }
            }
            
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.goDrawer:
                    if (_indexView != null)
                    {
                        _drawer?.OpenDrawer(GravityCompat.Start);
                    }
                    break;
                case Resource.Id.home:
                    if (_currentMapViewType == MapViewType.Home) return;
                    _currentMapViewType = MapViewType.Home;
                    SetIcon();

                    var homeTransaction = FragmentManager.BeginTransaction();
                    HideFragments(homeTransaction);
                    if (_homeFragment == null)
                    {
                        _homeFragment = new CategoryMapFragment();
                        var bundle = new Bundle();
                        _homeFragment.Arguments = bundle;
                        homeTransaction.Add(Resource.Id.contentLayout, _homeFragment);
                    }
                    else
                    {
                        homeTransaction.Show(_homeFragment);
                    }
                    homeTransaction.Commit();
                    break;
                case Resource.Id.build:
                    if (_currentMapViewType == MapViewType.Build) return;
                    _currentMapViewType = MapViewType.Build;
                    SetIcon();

                    var buildTransaction = FragmentManager.BeginTransaction();
                    HideFragments(buildTransaction);
                    if (_buildFragment == null)
                    {
                        _buildFragment = new CategoryMapFragment();
                        var bundle = new Bundle();
                        bundle.PutString(CategoryMapFragment.CategorymapProjecttype, ActivityConts.ProjectTypeBuild + "");
                        _buildFragment.Arguments = bundle;

                        buildTransaction.Add(Resource.Id.contentLayout, _buildFragment);
                    }
                    else
                    {
                        buildTransaction.Show(_buildFragment);
                    }
                    buildTransaction.Commit();
                    break;
                case Resource.Id.municipal:
                    if (_currentMapViewType == MapViewType.Municipal) return;
                    _currentMapViewType = MapViewType.Municipal;
                    SetIcon();

                    var municipalTransaction = FragmentManager.BeginTransaction();
                    HideFragments(municipalTransaction);
                    if (_municipalFragment == null)
                    {
                        _municipalFragment = new CategoryMapFragment();
                        var bundle = new Bundle();
                        bundle.PutString(CategoryMapFragment.CategorymapProjecttype, ActivityConts.ProjectTypeMunicipal + "");
                        _municipalFragment.Arguments = bundle;

                        municipalTransaction.Add(Resource.Id.contentLayout, _municipalFragment);
                    }
                    else
                    {
                        municipalTransaction.Show(_municipalFragment);
                    }
                    municipalTransaction.Commit();
                    break;
                case Resource.Id.mixing:
                    if (_currentMapViewType == MapViewType.Mixing) return;
                    _currentMapViewType = MapViewType.Mixing;
                    SetIcon();

                    var mixingTransaction = FragmentManager.BeginTransaction();
                    HideFragments(mixingTransaction);
                    if (_mixingFragment == null)
                    {
                        _mixingFragment = new CategoryMapFragment();
                        var bundle = new Bundle();
                        bundle.PutString(CategoryMapFragment.CategorymapProjecttype, ActivityConts.ProjectTypeMixing + "");
                        _mixingFragment.Arguments = bundle;

                        mixingTransaction.Add(Resource.Id.contentLayout, _mixingFragment);
                    }
                    else
                    {
                        mixingTransaction.Show(_mixingFragment);
                    }
                    mixingTransaction.Commit();
                    break;
                case Resource.Id.CascadeElement:
                    var cascadeIntent = new Intent(this, typeof(CascadeElementActivity));
                    var cascadeBundle = new Bundle();
                    cascadeBundle.PutInt(CascadeElementActivity.CascadeElementLevel, (int) ObjectType.WholeCity);
                    cascadeIntent.PutExtras(cascadeBundle);
                    StartActivity(cascadeIntent);
                    break;
                case Resource.Id.Statistics:
                    var statisticsIntent = new Intent(this, typeof(StatisticsActivity));
                    var statisticsBundle = new Bundle();
                    var projectType = string.Empty;
                    switch (_currentMapViewType)
                    {
                            case MapViewType.Build:
                                projectType = $"{ActivityConts.ProjectTypeBuild}";
                                break;
                            case MapViewType.Municipal:
                                projectType = $"{ActivityConts.ProjectTypeMunicipal}";
                            break;
                            case MapViewType.Mixing:
                                projectType = $"{ActivityConts.ProjectTypeMixing}";
                            break;
                    }
                    statisticsBundle.PutString(StatisticsActivity.NameStatisticsName, "全部区县");
                    statisticsBundle.PutString(StatisticsActivity.NameStatisticsType, projectType);
                    statisticsIntent.PutExtras(statisticsBundle);
                    StartActivity(statisticsIntent);
                    break;
                case Resource.Id.search:
                    _searchDialog.Show();
                    break;
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var id = item.ItemId;

            if (id == Resource.Id.standard)
            {
                StartActivity(new Intent(this, typeof(ReferenceStandardActivity)));
            }
            else if (id == Resource.Id.versionupdate)
            {
                _isCheckByUser = true;
            }
            else if (id == Resource.Id.exit)
            {
                AuthticationManager.Instance.Logout();
                StartActivity(new Intent(this, typeof(LoginActivity)));
                Finish();
            }

            _drawer?.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public void OnSearchClick(SearchResult searchResult)
        {
            _searchDialog.Dismiss();
            var objectLevell = (ObjectType)searchResult.objectLevel;
            switch (objectLevell)
            {
                case ObjectType.Project:
                    break;
                case ObjectType.Enterprise:
                    break;
                case ObjectType.District:
                    var intent = new Intent(this, typeof(StatisticsActivity));
                    var bundle = new Bundle();
                    bundle.PutString(StatisticsActivity.NameStatisticsName, searchResult.objectName);
                    bundle.PutString(StatisticsActivity.NameStatisticsElementId, searchResult.objectId);
                    bundle.PutString(StatisticsActivity.NameStatisticsElementType, searchResult.objectLevel + "");
                    bundle.PutBoolean(StatisticsActivity.NameStatisticsFromSearch, true);
                    intent.PutExtras(bundle);
                    StartActivity(intent);
                    break;
                case ObjectType.Device:
                    var intentDev = new Intent(this, typeof(DeviceDetailActivity));
                    var bundleDev = new Bundle();
                    bundleDev.PutString(ActivityConts.NameDeviceName, searchResult.objectName);
                    bundleDev.PutString(ActivityConts.NameDeviceId, searchResult.objectId);
                    intentDev.PutExtras(bundleDev);
                    StartActivity(intentDev);
                    break;
            }
        }

        private void SetIcon()
        {
            switch (_currentMapViewType)
            {
                case MapViewType.Home:
                    _homeView.SetImageResource(Resource.Mipmap.home_yes);
                    _buildView.SetImageResource(Resource.Mipmap.build_no);
                    _municipalView.SetImageResource(Resource.Mipmap.municipal_no);
                    _mixingView.SetImageResource(Resource.Mipmap.mixingplant_no);
                    break;
                case MapViewType.Build:
                    _homeView.SetImageResource(Resource.Mipmap.home_no);
                    _buildView.SetImageResource(Resource.Mipmap.build_yes);
                    _municipalView.SetImageResource(Resource.Mipmap.municipal_no);
                    _mixingView.SetImageResource(Resource.Mipmap.mixingplant_no);
                    break;
                case MapViewType.Municipal:
                    _homeView.SetImageResource(Resource.Mipmap.home_no);
                    _buildView.SetImageResource(Resource.Mipmap.build_no);
                    _municipalView.SetImageResource(Resource.Mipmap.municipal_yes);
                    _mixingView.SetImageResource(Resource.Mipmap.mixingplant_no);
                    break;
                case MapViewType.Mixing:
                    _homeView.SetImageResource(Resource.Mipmap.home_no);
                    _buildView.SetImageResource(Resource.Mipmap.build_no);
                    _municipalView.SetImageResource(Resource.Mipmap.municipal_no);
                    _mixingView.SetImageResource(Resource.Mipmap.mixingplant_yes);
                    break;
            }
        }

        private void HideFragments(FragmentTransaction transaction)
        {
            if (_homeFragment != null)
            {
                transaction.Hide(_homeFragment);
            }
            if (_buildFragment != null)
            {
                transaction.Hide(_buildFragment);
            }
            if (_municipalFragment != null)
            {
                transaction.Hide(_municipalFragment);
            }
            if (_mixingFragment != null)
            {
                transaction.Hide(_mixingFragment);
            }
        }
    }
}

