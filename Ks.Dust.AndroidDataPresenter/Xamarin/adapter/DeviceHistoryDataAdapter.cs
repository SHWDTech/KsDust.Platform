using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    class DeviceHistoryDataAdapter : AbstractRecyclerAdapter<DeviceHistoryData>
    {
        public DeviceHistoryDataAdapter(Context context) : base(context)
        {
        }

        public DeviceHistoryDataAdapter(Context context, List<DeviceHistoryData> adapterData) : base(context, adapterData)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var adapterViewHolder = holder as DeviceHistoryDataAdapterViewHolder;
            if (adapterViewHolder == null) return;
            var viewHolder = adapterViewHolder;
            var historyData = AdapterData[position];
            viewHolder.NoTextView.Text = position + "";
            viewHolder.TimeTextView.Text = historyData.date;
            viewHolder.TspTextView.Text = $"{historyData.tsp}";
            switch (historyData.rate) {
                case 0:
                    viewHolder.TspTextView.SetBackgroundResource(Resource.Drawable.tsp_good_bg);
                    break;
                case 1:
                    viewHolder.TspTextView.SetBackgroundResource(Resource.Drawable.tsp_normal_bg);
                    break;
                case 2:
                    viewHolder.TspTextView.SetBackgroundResource(Resource.Drawable.tsp_bad_bg);
                    break;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new DeviceHistoryDataAdapterViewHolder(LayoutInflater.Inflate(Resource.Layout.item_historydata, parent, false));
        }
    }

    class DeviceHistoryDataAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView NoTextView { get; }

        public TextView TimeTextView { get; }

        public TextView TspTextView { get; }

        public DeviceHistoryDataAdapterViewHolder(View itemView) : base(itemView)
        {
            NoTextView = (TextView)itemView.FindViewById(Resource.Id.item_historydata_no_tv);
            TimeTextView = (TextView)itemView.FindViewById(Resource.Id.item_historydata_time_tv);
            TspTextView = (TextView)itemView.FindViewById(Resource.Id.item_historydata_tsp_tv);
        }
    }
}