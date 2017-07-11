using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ApplicationConcept;
using CheeseBind;
using Ks.Dust.AndroidDataPresenter.Xamarin.activity;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Newtonsoft.Json;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.fragment
{
    public class DeviceCurrentDataFragment : Fragment, View.IOnClickListener
    {
        private string _currentDeviceId;

        private DeviceCurrentInfo _deviceCurrentInfo;

        [BindView(Resource.Id.name)]
        private TextView _name;

        [BindView(Resource.Id.address)]
        private TextView _address;

        [BindView(Resource.Id.vendor)]
        private TextView _vendor;

        [BindView(Resource.Id.enterprise)]
        private TextView _enterprise;

        [BindView(Resource.Id.superintend)]
        private TextView _superintend;

        [BindView(Resource.Id.mobile)]
        private TextView _mobile;

        [BindView(Resource.Id.isOnline)]
        private TextView _isOnline;

        [BindView(Resource.Id.tsp)]
        private TextView _tsp;

        [BindView(Resource.Id.pm25)]
        private TextView _pm25;

        [BindView(Resource.Id.pm100)]
        private TextView _pm100;

        [BindView(Resource.Id.noise)]
        private TextView _noise;

        [BindView(Resource.Id.temperature)]
        private TextView _temperature;

        [BindView(Resource.Id.humidity)]
        private TextView _humidity;

        [BindView(Resource.Id.windspeed)]
        private TextView _windspeed;

        [BindView(Resource.Id.winddirection)]
        private TextView _winddirection;

        [BindView(Resource.Id.updatetime)]
        private TextView _updatetime;

        [BindView(Resource.Id.camera)]
        private View _camera;

        private readonly Activity _ownerActivity;

        public DeviceCurrentDataFragment(Activity activity)
        {
            _ownerActivity = activity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            _currentDeviceId = Arguments.GetString(ActivityConts.NameDeviceId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_currentdata, container, false);
            Cheeseknife.Bind(this, view);
            _camera.SetOnClickListener(this);
            var handler = new HttpResponseHandler();
            handler.OnResponse += args =>
            {
                _deviceCurrentInfo = JsonConvert.DeserializeObject<DeviceCurrentInfo>(args.Response);
                _ownerActivity.RunOnUiThread(UpdateData);
            };
            ApiManager.GetDeviceCurrentData(_currentDeviceId, AuthticationManager.Instance.AccessToken, handler);
            return view;
        }

        private void UpdateData()
        {
            _name.Text = $"监测点：{_deviceCurrentInfo.name}";
            _address.Text = $"地址：{_deviceCurrentInfo.address}"; 
            _vendor.Text = $"供应商：{_deviceCurrentInfo.vendor}"; 
            _enterprise.Text = $"施工单位：{_deviceCurrentInfo.enterprise}"; 
            _superintend.Text = $"负责人：{_deviceCurrentInfo.superintend}"; 
            _mobile.Text = $"联系电话：{_deviceCurrentInfo.mobile}";
            _isOnline.Text = $"是否在线：{(_deviceCurrentInfo.isOnline ? "是" : "否")}"; 
            _tsp.Text = $"tsp：{_deviceCurrentInfo.tsp}mg/m³";
            _pm25.Text = $"pm2.5：{_deviceCurrentInfo.pm25}ug/m³";
            _pm100.Text = $"pm10：{_deviceCurrentInfo.pm100}ug/m³";
            _noise.Text = $"噪声：{_deviceCurrentInfo.noise}dB"; 
            _temperature.Text = $"温度：{_deviceCurrentInfo.temperature}℃";
            _humidity.Text = $"湿度：{_deviceCurrentInfo.humidity}%";
            _windspeed.Text = $"风速：{_deviceCurrentInfo.windspeed}m/s";
            _winddirection.Text = $"风向：{_deviceCurrentInfo.winddirection}°";
            _updatetime.Text = $"{_deviceCurrentInfo.updatetime}";
            if (!string.IsNullOrWhiteSpace(_deviceCurrentInfo.serialNumber))
            {
                _camera.Visibility = ViewStates.Visible;
            }
        }

        public void OnClick(View v)
        {
            Toast.MakeText(Activity, "正在加载,请稍候", ToastLength.Short).Show();
            var intent = new Intent(Activity, typeof(DeviceCameraActivity));
            var bundle = new Bundle();
            bundle.PutString(DeviceCameraActivity.Ip, _deviceCurrentInfo.ipServerAddr);
            bundle.PutString(DeviceCameraActivity.Serialnumber, _deviceCurrentInfo.serialNumber);
            bundle.PutString(DeviceCameraActivity.UserName, _deviceCurrentInfo.userName);
            bundle.PutString(DeviceCameraActivity.UserPassword, _deviceCurrentInfo.password);
            intent.PutExtras(bundle);
            StartActivity(intent);
        }
    }
}