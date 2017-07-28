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
        protected TextView Name;

        [BindView(Resource.Id.address)]
        protected TextView Address;

        [BindView(Resource.Id.vendor)]
        protected TextView Vendor;

        [BindView(Resource.Id.enterprise)]
        protected TextView Enterprise;

        [BindView(Resource.Id.superintend)]
        protected TextView Superintend;

        [BindView(Resource.Id.mobile)]
        protected TextView Mobile;

        [BindView(Resource.Id.isOnline)]
        protected TextView IsOnline;

        [BindView(Resource.Id.tsp)]
        protected TextView Tsp;

        [BindView(Resource.Id.pm25)]
        protected TextView Pm25;

        [BindView(Resource.Id.pm100)]
        protected TextView Pm100;

        [BindView(Resource.Id.noise)]
        protected TextView Noise;

        [BindView(Resource.Id.temperature)]
        protected TextView Temperature;

        [BindView(Resource.Id.humidity)]
        protected TextView Humidity;

        [BindView(Resource.Id.windspeed)]
        protected TextView Windspeed;

        [BindView(Resource.Id.winddirection)]
        protected TextView Winddirection;

        [BindView(Resource.Id.updatetime)]
        protected TextView Updatetime;

        [BindView(Resource.Id.camera)]
        protected View Camera;

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
            Camera.SetOnClickListener(this);
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
            Name.Text = $"监测点：{_deviceCurrentInfo.name}";
            Address.Text = $"地址：{_deviceCurrentInfo.address}"; 
            Vendor.Text = $"供应商：{_deviceCurrentInfo.vendor}"; 
            Enterprise.Text = $"施工单位：{_deviceCurrentInfo.enterprise}"; 
            Superintend.Text = $"负责人：{_deviceCurrentInfo.superintend}"; 
            Mobile.Text = $"联系电话：{_deviceCurrentInfo.mobile}";
            IsOnline.Text = $"是否在线：{(_deviceCurrentInfo.isOnline ? "是" : "否")}"; 
            Tsp.Text = $"tsp：{_deviceCurrentInfo.tsp}mg/m³";
            Pm25.Text = $"pm2.5：{_deviceCurrentInfo.pm25}ug/m³";
            Pm100.Text = $"pm10：{_deviceCurrentInfo.pm100}ug/m³";
            Noise.Text = $"噪声：{_deviceCurrentInfo.noise}dB"; 
            Temperature.Text = $"温度：{_deviceCurrentInfo.temperature}℃";
            Humidity.Text = $"湿度：{_deviceCurrentInfo.humidity}%";
            Windspeed.Text = $"风速：{_deviceCurrentInfo.windspeed}m/s";
            Winddirection.Text = $"风向：{_deviceCurrentInfo.winddirection}°";
            Updatetime.Text = $"{_deviceCurrentInfo.updatetime}";
            if (!string.IsNullOrWhiteSpace(_deviceCurrentInfo.serialNumber))
            {
                Camera.Visibility = ViewStates.Visible;
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