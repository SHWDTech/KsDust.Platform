using System;
using Android.App;
using Android.OS;
using Android.Views;
using Com.Amap.Api.Maps2d;
using Com.Amap.Api.Maps2d.Model;
using ApplicationConcept;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class MapFragment : Fragment, AMap.IOnMarkerClickListener, AMap.IOnInfoWindowClickListener, AMap.IInfoWindowAdapter
    {
        private ProjectType _projectType;

        private string _districtId;

        public const string CategorymapProjecttype = "Project";

        public const string CategorymapDistrictid = "District";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _projectType = (ProjectType)Arguments.GetInt(CategorymapProjecttype, 0xFF);
            _districtId = Arguments.GetString(CategorymapDistrictid);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.fragment_category_map, container, false);
        }

        public void LoadMapInfo()
        {
            if (string.IsNullOrWhiteSpace(_districtId))
            {

            }
            else
            {

            }
        }

        public bool OnMarkerClick(Marker p0)
        {
            throw new NotImplementedException();
        }

        public void OnInfoWindowClick(Marker p0)
        {
            throw new NotImplementedException();
        }

        public View GetInfoContents(Marker p0)
        {
            throw new NotImplementedException();
        }

        public View GetInfoWindow(Marker p0)
        {
            throw new NotImplementedException();
        }
    }
}