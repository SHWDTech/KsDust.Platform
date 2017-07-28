using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    class StatisticsBarChartAdapter : AbstractRecyclerAdapter<DistrictGeneralInfo>
    {
        public double MaxValue { get; set; }

        private readonly int _reclyerViewHeight;


        public StatisticsBarChartAdapter(Context context) : base(context)
        {
            _reclyerViewHeight = context.Resources.GetDimensionPixelSize(Resource.Dimension.custom_chart_height);
        }

        public StatisticsBarChartAdapter(Context context, List<DistrictGeneralInfo> adapterData) : base(context, adapterData)
        {
            AdapterData = adapterData;
            _reclyerViewHeight = context.Resources.GetDimensionPixelSize(Resource.Dimension.custom_chart_height);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is StatisticsBarChartAdapterViewHolder)
            {
                var customViewHolder = (StatisticsBarChartAdapterViewHolder)holder;
                var data = AdapterData[position];
                customViewHolder.BarChartValueTextView.Text = $"{data.tspAvg}";
                customViewHolder.BarChartBottomTv.Text = data.name;
                customViewHolder.BarChartStateView.SetBackgroundColor(Color.Green);
                var contentHeight = (int)((data.tspAvg / MaxValue) * 0.7 * _reclyerViewHeight);
                var layoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, contentHeight);
                layoutParams.AddRule(LayoutRules.Above, Resource.Id.BarChartBottomTv);
                customViewHolder.BarChartContentView.LayoutParameters = layoutParams;
                switch (data.rate)
                {
                    case ActivityConts.RateGood:
                        customViewHolder.BarChartStateView.SetBackgroundResource(Resource.Drawable.tsp_good_bg);
                        break;
                    case ActivityConts.RateNormal:
                        customViewHolder.BarChartStateView.SetBackgroundResource(Resource.Drawable.tsp_normal_bg);
                        break;
                    case ActivityConts.RateBad:
                        customViewHolder.BarChartStateView.SetBackgroundResource(Resource.Drawable.tsp_bad_bg);
                        break;
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new StatisticsBarChartAdapterViewHolder(LayoutInflater.Inflate(Resource.Layout.item_statistics_barchart, parent, false));
        }
    }

    class StatisticsBarChartAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        public TextView BarChartValueTextView { get; set; }

        public View BarChartStateView { get; set; }

        public View BarChartContentView { get; set; }

        public TextView BarChartBottomTv { get; set; }

        public StatisticsBarChartAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public StatisticsBarChartAdapterViewHolder(View itemView) : base(itemView)
        {
            BarChartValueTextView = (TextView)itemView.FindViewById(Resource.Id.BarChartValueTextView);
            BarChartStateView = itemView.FindViewById(Resource.Id.BarChartStateView);
            BarChartContentView = itemView.FindViewById(Resource.Id.BarChartContentView);
            BarChartBottomTv = (TextView)itemView.FindViewById(Resource.Id.BarChartBottomTv);
        }

        public void OnClick(View v)
        {
            
        }
    }
}