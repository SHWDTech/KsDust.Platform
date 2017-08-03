﻿using Android.Content;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.receiver
{
    public delegate void BroadcastReceived(Context context, Intent intent);

    public class DownloadReceiver : BroadcastReceiver
    {
        public event BroadcastReceived OnReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            OnReceived?.Invoke(context, intent);
        }
    }
}