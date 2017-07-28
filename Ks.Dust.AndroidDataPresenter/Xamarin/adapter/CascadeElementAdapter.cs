using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    public class CascadeElementAdapter : AbstractRecyclerAdapter<CascadeElement>
    {
        public IOnCascadeElementItemListener OnCascadeElementItemListener { get; set; }

        public CascadeElementAdapter(Context context) : base(context)
        {
            
        }

        public CascadeElementAdapter(Context context, List<CascadeElement> adapterData) : base(context, adapterData)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as CascadeElementViewHolder;
            if (viewHolder != null)
            {
                var cascadeElementViewHolder = viewHolder;
                var cascadeElement = AdapterData[position];
                cascadeElementViewHolder.CascadeElementName.Text = cascadeElement.cascadeElementName;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var holder =
                new CascadeElementViewHolder(
                    LayoutInflater.Inflate(Resource.Layout.item_cascade_element, parent, false))
                {
                    OnCascadeElementItemListener = OnCascadeElementItemListener,
                    AdapterData = AdapterData
                };
            return holder;
        }

    }

    public class CascadeElementViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        public TextView CascadeElementName { get; }

        public IOnCascadeElementItemListener OnCascadeElementItemListener { get; set; }

        public List<CascadeElement> AdapterData { get; set; }

        public CascadeElementViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CascadeElementViewHolder(CascadeElementAdapter adapter, View itemView) : this(itemView)
        {
            OnCascadeElementItemListener = adapter.OnCascadeElementItemListener;
            AdapterData = adapter.AdapterData;
        }

        public CascadeElementViewHolder(View itemView) : base(itemView)
        {
            itemView.FindViewById(Resource.Id.CascadeElementBg).SetOnClickListener(this);
            CascadeElementName = (TextView) itemView.FindViewById(Resource.Id.CascadeElementName);
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.CascadeElementBg:
                    if (OnCascadeElementItemListener != null)
                    {
                        if (AdapterPosition < AdapterData.Count && AdapterPosition >= 0)
                        {
                            OnCascadeElementItemListener.OnCascadeElementItemClick(AdapterPosition);
                        }
                    }
                    break;
            }
        }
    }

    public interface IOnCascadeElementItemListener
    {
        void OnCascadeElementItemClick(int position);
    }

    public class OnCascadeElementItemListener : IOnCascadeElementItemListener
    {
        private readonly Action _action;

        public OnCascadeElementItemListener(Action action)
        {
            _action = action;
        }

        public void OnCascadeElementItemClick(int position)
        {
            _action?.Invoke();
        }
    }
}