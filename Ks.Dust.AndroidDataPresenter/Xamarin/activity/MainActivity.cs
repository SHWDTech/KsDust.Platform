using System;
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
        protected DrawerLayout Drawer;

        [BindView(Resource.Id.goDrawer)]
        protected View IndexView;

        [BindView(Resource.Id.home)]
        protected ImageView HomeView;

        [BindView(Resource.Id.build)]
        protected ImageView BuildView;

        [BindView(Resource.Id.municipal)]
        protected ImageView MunicipalView;

        [BindView(Resource.Id.mixing)]
        protected ImageView MixingView;

        [BindView(Resource.Id.title)]
        protected TextView CityTitleView;

        [BindView(Resource.Id.CascadeElement)]
        protected View CascadeElementView;

        [BindView(Resource.Id.Statistics)]
        protected View StatisticsView;

        private MapViewType _currentMapViewType = MapViewType.Home;

        private CategoryMapFragment _homeFragment, _buildFragment, _municipalFragment, _mixingFragment;

        protected PopupWindow IndexPopupWindow;

        [BindView(Resource.Id.search)]
        protected View SearchView;

        private SearchDialog _searchDialog;

        protected bool IsCheckByUser;

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
            IndexView.SetOnClickListener(this);
            HomeView.SetOnClickListener(this);
            BuildView.SetOnClickListener(this);
            MunicipalView.SetOnClickListener(this);
            MixingView.SetOnClickListener(this);
            CascadeElementView.SetOnClickListener(this);
            StatisticsView.SetOnClickListener(this);
            CityTitleView.Text = "昆山";
            SearchView.SetOnClickListener(this);
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
            if (Drawer.IsDrawerOpen(GravityCompat.Start))
            {
                Drawer.CloseDrawer(GravityCompat.Start);
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
                    if (IndexView != null)
                    {
                        Drawer?.OpenDrawer(GravityCompat.Start);
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
                IsCheckByUser = true;
            }
            else if (id == Resource.Id.exit)
            {
                AuthticationManager.Instance.Logout();
                StartActivity(new Intent(this, typeof(LoginActivity)));
                Finish();
            }

            Drawer?.CloseDrawer(GravityCompat.Start);
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
                    HomeView.SetImageResource(Resource.Mipmap.home_yes);
                    BuildView.SetImageResource(Resource.Mipmap.build_no);
                    MunicipalView.SetImageResource(Resource.Mipmap.municipal_no);
                    MixingView.SetImageResource(Resource.Mipmap.mixingplant_no);
                    break;
                case MapViewType.Build:
                    HomeView.SetImageResource(Resource.Mipmap.home_no);
                    BuildView.SetImageResource(Resource.Mipmap.build_yes);
                    MunicipalView.SetImageResource(Resource.Mipmap.municipal_no);
                    MixingView.SetImageResource(Resource.Mipmap.mixingplant_no);
                    break;
                case MapViewType.Municipal:
                    HomeView.SetImageResource(Resource.Mipmap.home_no);
                    BuildView.SetImageResource(Resource.Mipmap.build_no);
                    MunicipalView.SetImageResource(Resource.Mipmap.municipal_yes);
                    MixingView.SetImageResource(Resource.Mipmap.mixingplant_no);
                    break;
                case MapViewType.Mixing:
                    HomeView.SetImageResource(Resource.Mipmap.home_no);
                    BuildView.SetImageResource(Resource.Mipmap.build_no);
                    MunicipalView.SetImageResource(Resource.Mipmap.municipal_no);
                    MixingView.SetImageResource(Resource.Mipmap.mixingplant_yes);
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

