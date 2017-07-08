using System;
using Android.App;
using Android.Runtime;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.appplication
{
    [Application(Label = "昆山扬尘在线监控平台")]
    public class KsDustApplication : Application
    {
        public KsDustApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public override void OnCreate()
        {
            base.OnCreate();
            AuthticationManager.Init(this);
        }
    }
}