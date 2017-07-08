using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view
{
    public class SearchDialog : Dialog
    {
        private EditText _editText;

        private RecyclerView _recyclerView;

        private List<SearchResult> _searchResults = new List<SearchResult>();



        public SearchDialog(Context context, bool cancelable, EventHandler cancelHandler) : base(context, cancelable, cancelHandler)
        {
        }

        public SearchDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SearchDialog(Context context) : this(context, Resource.Style.chat_dialog)
        {
        }

        public SearchDialog(Context context, bool cancelable, IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener)
        {
        }

        public SearchDialog(Context context, int themeResId) : base(context, themeResId)
        {
            
        }
    }
}