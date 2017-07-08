using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    public abstract class AbstractRecyclerAdapter<T> : RecyclerView.Adapter
    {
        public List<T> AdapterData { get; private set; }

        protected LayoutInflater _layoutInflater;

        protected Context _context;

        protected AbstractRecyclerAdapter(Context context)
        {
            _context = context;
            _layoutInflater = LayoutInflater.From(_context);
        }

        public override int ItemCount => AdapterData?.Count ?? 0;
    }
}