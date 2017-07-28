using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    public class StatisticsRecyclerAdapter : AbstractRecyclerAdapter<DistrictGeneralInfo>
    {
        public IOnStatisticsRecyclerItemClickListener OnItemClickListener { get; set; }

        public StatisticsRecyclerAdapter(Context context) : base(context)
        {
        }

        public StatisticsRecyclerAdapter(Context context, List<DistrictGeneralInfo> adapterData) : base(context, adapterData)
        {
            AdapterData = adapterData;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is StatisticsRecyclerAdapterViewHolder)
            {
                var viewHolder = (StatisticsRecyclerAdapterViewHolder)holder;
                var districtInfo = AdapterData[position];
                viewHolder.DistrictavgItemNameTextView.Text = districtInfo.name;
                viewHolder.DistrictavgCountTextView.Text = $"{districtInfo.count}";
                viewHolder.DistrictavgTspavgTextView.Text = $"{districtInfo.tspAvg}";

                switch (districtInfo.rate)
                {
                    case ActivityConts.RateGood:
                        viewHolder.DistrictavgTspavgTextView.SetBackgroundResource(Resource.Drawable.tsp_good_bg);
                        break;
                    case ActivityConts.RateNormal:
                        viewHolder.DistrictavgTspavgTextView.SetBackgroundResource(Resource.Drawable.tsp_normal_bg);
                        break;
                    case ActivityConts.RateBad:
                        viewHolder.DistrictavgTspavgTextView.SetBackgroundResource(Resource.Drawable.tsp_bad_bg);
                        break;
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var holder =
                new StatisticsRecyclerAdapterViewHolder(
                    LayoutInflater.Inflate(Resource.Layout.item_statistics_recyler, parent, false))
                {
                    OnItemClickListener = OnItemClickListener,
                    AdapterData = AdapterData
                };
            return holder;
        }
    }

    public class StatisticsRecyclerAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        public TextView DistrictavgItemNameTextView { get; }

        public TextView DistrictavgCountTextView { get; }

        public TextView DistrictavgTspavgTextView { get; }

        public View BackGroundView { get; }

        public IOnStatisticsRecyclerItemClickListener OnItemClickListener { get; set; }

        public List<DistrictGeneralInfo> AdapterData { get; set; }

        public StatisticsRecyclerAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public StatisticsRecyclerAdapterViewHolder(View itemView) : base(itemView)
        {
            BackGroundView = itemView.FindViewById(Resource.Id.county_item_bg);
            BackGroundView.SetOnClickListener(this);
            DistrictavgItemNameTextView = (TextView) itemView.FindViewById(Resource.Id.districtavg_item_name_tv);
            DistrictavgCountTextView = (TextView)itemView.FindViewById(Resource.Id.districtavg_item_count_tv);
            DistrictavgTspavgTextView = (TextView)itemView.FindViewById(Resource.Id.districtavg_item_tspavg_tv);
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.county_item_bg:
                    if (OnItemClickListener != null)
                    {
                        if (AdapterPosition < AdapterData.Count && AdapterData.Count >= 0)
                        {
                            OnItemClickListener.OnStatisticsItemClick(AdapterPosition);
                        }
                    }
                    break;
            }
        }
    }

    public interface IOnStatisticsRecyclerItemClickListener
    {
        void OnStatisticsItemClick(int position);
    }

    public class OnStatisticsRecyclerItemClickListener : Java.Lang.Object, IOnStatisticsRecyclerItemClickListener
    {
        private readonly Action _action;

        public OnStatisticsRecyclerItemClickListener(Action action)
        {
            _action = action;
        }

        public void OnStatisticsItemClick(int position)
        {
            _action?.Invoke();
        }
    }
}