using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    public abstract class AbstractRecyclerAdapter<T> : RecyclerView.Adapter
    {
        public List<T> AdapterData { get; set; }

        protected LayoutInflater LayoutInflater;

        protected Context Context;

        protected AbstractRecyclerAdapter(Context context)
        {
            Context = context;
            LayoutInflater = LayoutInflater.From(Context);
        }

        protected AbstractRecyclerAdapter(Context context, List<T> adapterData)
        {
            Context = context;
            LayoutInflater = LayoutInflater.From(Context);
            AdapterData = adapterData;
        }

        public override int ItemCount => AdapterData?.Count ?? 0;
    }
}