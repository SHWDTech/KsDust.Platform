using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.fragment;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = "Ks.Dust.AndroidDataPresenter", Icon = "@drawable/icon")]
    public class MainActivity : KsDustBaseActivity
    {
        [BindView(Resource.Id.main_layout)]
        private LinearLayout _mainLayout;

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

        private CategoryMapFragment _homeFragment, _buildFragment, _municipalFragment, mixingFragment;

        private List<string> _cityList = new List<string>();

        private PopupWindow _indexPopupWindow;

        [BindView(Resource.Id.search)]
        private View _searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Bind(this);
        }
    }
}

