using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ApplicationConcept;
using Com.Amap.Api.Maps2d;
using Com.Amap.Api.Maps2d.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.activity;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Newtonsoft.Json;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class CategoryMapFragment : Fragment, AMap.IOnMarkerClickListener, AMap.IOnInfoWindowClickListener, AMap.IInfoWindowAdapter
    {
        private string _objectType;

        private string _districtId;

        public const string CategorymapProjecttype = "Project";

        public const string CategorymapDistrictid = "District";

        private MapView _mapView;

        private AMap _map;

        public static readonly LatLng Kunshan = new LatLng(31.238068, 121.501654);

        private readonly Dictionary<LatLng, MapMarker> _mapMarkers = new Dictionary<LatLng, MapMarker>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _objectType = Arguments.GetString(CategorymapProjecttype, string.Empty);
            _districtId = Arguments.GetString(CategorymapDistrictid);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            return inflater.Inflate(Resource.Layout.fragment_category_map, container, false); ;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            _mapView = (MapView)View.FindViewById(Resource.Id.map);
            _mapView.OnCreate(savedInstanceState);
            _map = _mapView.Map;
            _map.SetOnInfoWindowClickListener(this);
            _map.SetInfoWindowAdapter(this);
            _map.SetOnMarkerClickListener(this);
            var bounds = new LatLngBounds.Builder().Include(Kunshan).Build();
            _map.MoveCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 10));
            var handler = new HttpResponseHandler();
            if (string.IsNullOrWhiteSpace(_districtId))
            {
                handler.OnResponse += args =>
                {
                    var devList = JsonConvert.DeserializeObject<List<MapMarker>>(args.Response);
                    AddMarker(devList);
                };
                ApiManager.GetDeviceLocationByProjectType(_objectType, AuthticationManager.Instance.AccessToken, handler);
            }
            else
            {
                handler.OnResponse += args =>
                {
                    var devList = JsonConvert.DeserializeObject<List<MapMarker>>(args.Response);
                    AddMarker(devList);
                };
                ApiManager.GetDeviceLocationByDistrictId(_objectType, _districtId, AuthticationManager.Instance.AccessToken, handler);
            }
        }

        public bool OnMarkerClick(Marker p0)
        {
            _map.MoveCamera(CameraUpdateFactory.ChangeLatLng(p0.Position));
            return false;
        }

        public void OnInfoWindowClick(Marker p0)
        {

        }

        public View GetInfoContents(Marker p0)
        {
            var infoContent = LayoutInflater.From(Activity).Inflate(Resource.Layout.marker_infowindow, null);
            return infoContent;
        }

        public View GetInfoWindow(Marker p0)
        {
            var infoContent = LayoutInflater.From(Activity).Inflate(Resource.Layout.marker_infowindow, null);
            RenderView(infoContent, p0);
            return infoContent;
        }

        private void AddMarker(List<MapMarker> mapMarkers)
        {
            _map.Clear();
            _mapMarkers.Clear();

            var latlngs = new List<LatLng>();

            foreach (var mapMarker in mapMarkers)
            {
                var markerOption = new MarkerOptions();
                if (mapMarker.latitude > 0 && mapMarker.longitude > 0)
                {
                    var latlng = new LatLng(mapMarker.latitude, mapMarker.longitude);
                    latlngs.Add(latlng);

                    markerOption.InvokePosition(latlng);
                    var textView = new TextView(Activity)
                    {
                        Text = $"{mapMarker.tsp}"
                    };
                    textView.SetPadding(10, 5, 10, 5);
                    textView.SetTextColor(Color.White);
                    textView.Gravity = GravityFlags.Center;
                    textView.SetMinWidth(120);

                    switch (mapMarker.rate)
                    {
                        case ActivityConts.RateGood:
                            textView.SetBackgroundResource(Resource.Drawable.tsp_good_bg);
                            break;
                        case ActivityConts.RateNormal:
                            textView.SetBackgroundResource(Resource.Drawable.tsp_normal_bg);
                            break;
                        case ActivityConts.RateBad:
                            textView.SetBackgroundResource(Resource.Drawable.tsp_bad_bg);
                            break;
                    }

                    markerOption.Draggable(true);
                    markerOption.InvokeTitle(string.Empty);
                    markerOption.InvokeIcon(BitmapDescriptorFactory.FromView(textView));
                    _mapMarkers.Add(latlng, mapMarker);
                    _map.AddMarker(markerOption);
                }

                var bounds = new LatLngBounds.Builder();
                foreach (var latlng in latlngs)
                {
                    bounds.Include(latlng);
                }
                var latlngBounds = bounds.Build();
                _map.MoveCamera(CameraUpdateFactory.NewLatLngBounds(latlngBounds, 10));
                _map.Invalidate();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _mapView.OnDestroy();
        }

        public override void OnResume()
        {
            base.OnResume();
            _mapView.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
            _mapView.OnPause();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            _mapView.OnSaveInstanceState(outState);
        }

        private void RenderView(View v, Marker marker)
        {
            if (_mapMarkers.ContainsKey(marker.Position))
            {
                var mapMarker = _mapMarkers[marker.Position];
                var titleView = (TextView) v.FindViewById(Resource.Id.marker_title);
                if (!string.IsNullOrWhiteSpace(mapMarker.name))
                {
                    titleView.Text = mapMarker.name;
                }
                var closeView = (ImageView) v.FindViewById(Resource.Id.marker_close);
                closeView.SetOnClickListener(new CloseViewClickListener(marker));
                var tspView = (TextView) v.FindViewById(Resource.Id.marker_tsp);
                tspView.Text = $"浓度：{mapMarker.tsp}mg/m³";
                var rateView = (TextView) v.FindViewById(Resource.Id.marker_rate);
                var backGround = v.FindViewById(Resource.Id.info_bg_layout);
                switch (mapMarker.rate)
                {
                    case ActivityConts.RateGood:
                        rateView.Text = "好";
                        rateView.SetTextColor(Resources.GetColor(Resource.Color.well_color));
                        backGround.SetBackgroundColor(new Color(Resource.Color.well_color));
                        break;
                    case ActivityConts.RateNormal:
                        rateView.Text = "中";
                        rateView.SetTextColor(Resources.GetColor(Resource.Color.middle_color));
                        backGround.SetBackgroundColor(new Color(Resource.Color.middle_color));
                        break;
                    case ActivityConts.RateBad:
                        rateView.Text = "差";
                        rateView.SetTextColor(Resources.GetColor(Resource.Color.bad_color));
                        backGround.SetBackgroundColor(new Color(Resource.Color.bad_color));
                        break;
                }
                var timeView = (TextView) v.FindViewById(Resource.Id.marker_time);
                timeView.Text = mapMarker.time;
                var infoLayout = v.FindViewById(Resource.Id.marker_infolayout);
                infoLayout.SetOnClickListener(new InfoLayoutClickListener(marker, Activity, mapMarker));
            }
        }
    }

    public class CloseViewClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Marker _marker;

        public CloseViewClickListener(Marker mapMarker)
        {
            _marker = mapMarker;
        }

        public void OnClick(View v)
        {
            _marker.HideInfoWindow();
        }
    }

    public class InfoLayoutClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Marker _marker;

        private readonly Activity _activity;

        private readonly MapMarker _mapMarker;

        public InfoLayoutClickListener(Marker marker, Activity activity, MapMarker mapMarker)
        {
            _marker = marker;
            _activity = activity;
            _mapMarker = mapMarker;
        }

        public void OnClick(View v)
        {
            _marker.HideInfoWindow();
            var intent = new Intent(_activity, typeof(DeviceDetailActivity));
            var bundle = new Bundle();
            bundle.PutString(ActivityConts.NameDeviceId, _mapMarker.id);
            bundle.PutString(ActivityConts.NameDeviceName, _mapMarker.name);
            intent.PutExtras(bundle);
            _activity.StartActivity(intent);
        }
    }
}