using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Java.Lang;
using Math = Java.Lang.Math;
using Object = Java.Lang.Object;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh
{
    public class WrapperBaseAdapter : RecyclerView.Adapter
    {
        public RecyclerView.Adapter BaseAdapter { get; }

        private static readonly int HeaderViewType = -1000;

        private readonly int FooterViewType = -2000;

        private List<View> Headers { get; } = new List<View>();

        private List<View> Footers { get; } = new List<View>();

        public int HeaderCount => Headers.Count;

        public int FooterCount => Footers.Count;

        public WrapperBaseAdapter(RecyclerView.Adapter adapter)
        {
            BaseAdapter = adapter;
        }

        public void AddHeader(View view)
        {
            if (view == null)
            {
                throw new IllegalArgumentException("You can't have a null header!");
            }
            Headers.Add(view);
        }

        public void AddFooter(View view)
        {
            if (view == null)
            {
                throw new IllegalArgumentException("You can't have a null header!");
            }
            Footers.Add(view);
        }

        public void SetHeaderVisibility(bool show)
        {
            foreach (var header in Headers)
            {
                header.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        public void SetFooterVisibility(bool show)
        {
            foreach (var footer in Footers)
            {
                footer.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        private bool IsHeader(int viewType)
        {
            return viewType >= HeaderViewType && viewType < HeaderViewType + Footers.Count;
        }

        private bool IsFooter(int viewType)
        {
            return viewType >= FooterViewType && viewType < FooterViewType + Footers.Count;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position < Headers.Count)
            {
                // Headers don't need anything special

            }
            else if (position < Headers.Count + BaseAdapter.ItemCount)
            {
                // This is a real position, not a header or footer. Bind it.
                BaseAdapter.OnBindViewHolder(holder, position - Headers.Count);

            }
            else
            {
                // Footers don't need anything special
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (IsHeader(viewType))
            {
                var whichHeader = Math.Abs(viewType - HeaderViewType);
                var headerView = Headers[whichHeader];
                return new WarpperViewHolder(headerView);
            }
            if (IsFooter(viewType))
            {
                var whichFooter = Math.Abs(viewType - FooterViewType);
                var headerView = Footers[whichFooter];
                return new WarpperViewHolder(headerView);
            }
            return BaseAdapter.OnCreateViewHolder(parent, viewType);
        }

        public override int ItemCount => Headers.Count + BaseAdapter.ItemCount + Footers.Count;

        public override int GetItemViewType(int position)
        {
            if (position < Headers.Count)
            {
                return HeaderViewType + position;

            }
            if (position < Headers.Count + BaseAdapter.ItemCount)
            {
                return BaseAdapter.GetItemViewType(position - Headers.Count);

            }
            return FooterViewType + position - Headers.Count - BaseAdapter.ItemCount;
        }

        public new bool HasStableIds => BaseAdapter.HasStableIds;

        public override long GetItemId(int position)
        {
            if (position < Headers.Count)
            {
                return -1;
            }
            if (position < Headers.Count + BaseAdapter.ItemCount)
            {
                return BaseAdapter.GetItemId(position - Headers.Count);
            }
            return -2;
        }

        public override void OnViewRecycled(Object holder)
        {
            BaseAdapter.OnViewRecycled(holder);
        }

        public override bool OnFailedToRecycleView(Object holder)
        {
            return BaseAdapter.OnFailedToRecycleView(holder);
        }

        public override void OnViewAttachedToWindow(Object holder)
        {
            BaseAdapter.OnViewAttachedToWindow(holder);
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            BaseAdapter.OnViewDetachedFromWindow(holder);
        }

        public override void RegisterAdapterDataObserver(RecyclerView.AdapterDataObserver observer)
        {
            if (observer == null) return;
            BaseAdapter.RegisterAdapterDataObserver(observer);
        }

        public override void UnregisterAdapterDataObserver(RecyclerView.AdapterDataObserver observer)
        {
            if (observer == null) return;
            BaseAdapter.UnregisterAdapterDataObserver(observer);
        }

        public override void OnAttachedToRecyclerView(RecyclerView recyclerView)
        {
            BaseAdapter.OnAttachedToRecyclerView(recyclerView);
        }

        public override void OnDetachedFromRecyclerView(RecyclerView recyclerView)
        {
            BaseAdapter.OnDetachedFromRecyclerView(recyclerView);
        }
    }

    public class WarpperViewHolder : RecyclerView.ViewHolder
    {
        public WarpperViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public WarpperViewHolder(View itemView) : base(itemView)
        {
        }
    }
}