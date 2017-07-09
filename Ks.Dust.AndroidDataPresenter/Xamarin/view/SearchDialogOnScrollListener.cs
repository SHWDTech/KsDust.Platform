using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view
{
    public class SearchDialogOnScrollListener : RecyclerView.OnScrollListener
    {
        private readonly RecyclerView _recyclerView;

        public SearchDialogOnScrollListener(SearchDialog searchDialog)
        {
            _recyclerView = searchDialog.RecyclerView;
        }

        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            switch (newState)
            {
                case RecyclerView.ScrollStateDragging:
                    HideInputMethod(_recyclerView);
                    break;

            }
        }

        private static void HideInputMethod(View view)
        {
            if (view == null) return;
            try
            {
                var imm = (InputMethodManager) view.Context.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromInputMethod(view.WindowToken, HideSoftInputFlags.NotAlways);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}