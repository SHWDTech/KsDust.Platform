using System;
using Android.Views;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view
{
    public class ClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Action _action;

        public ClickListener(Action action)
        {
            _action = action;
        }

        public void OnClick(View v)
        {
            _action.Invoke();
        }
    }
}