using Android.App;
using Android.OS;
using Android.Widget;
using CheeseBind;
using System;
using Ks.Dust.AndroidDataPresenter.Xamarin.fragment;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(DistrictDetailMapActivity))]
    public class DistrictDetailMapActivity : KsDustBaseActivity
    {
        private string _districtName;

        private TextView _titleView;

        public const string DistrictdetailmapDistrictname = "name";

        [OnClick(Resource.Id.back)]
        private void Back(object sender, EventArgs args)
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_disdetail_map);
            Cheeseknife.Bind(this);
            InitView();
            InitData();
        }

        private void InitView()
        {
            _titleView = (TextView) FindViewById((Resource.Id.title));
        }

        private void InitData()
        {
            var bundle = Intent.Extras;
            _districtName = bundle.GetString(DistrictdetailmapDistrictname);
            _titleView.Text = _districtName;

            var transaction = FragmentManager.BeginTransaction();
            var mapFragment = new CategoryMapFragment();
            mapFragment.Arguments = bundle;
            transaction.Add(Resource.Id.map_content, mapFragment).Commit();
        }
    }
}