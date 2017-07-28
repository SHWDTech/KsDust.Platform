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
    public class DistrictDetailAdapter : AbstractRecyclerAdapter<DistrictDetail>
    {
        public IOnDistrictDetailItemClickListener OnDistrictDetailItemClickListener { get; set; }

        public DistrictDetailAdapter(Context context) : base(context)
        {
        }

        public DistrictDetailAdapter(Context context, List<DistrictDetail> adapterData) : base(context, adapterData)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as DistrictDetailAdapterViewHolder;
            if (viewHolder == null) return;
            var detailViewHolder = viewHolder;
            var detail = AdapterData[position];
            detailViewHolder.ItemDistrictdetailNo.Text = $"{position}";
            detailViewHolder.ItemDistrictdetailCountyname.Text = detail.districtName.Trim();
            detailViewHolder.ItemDistrictdetailName.Text = detail.name;
            detailViewHolder.ItemDistrictdetailTsp.Text = $"{detail.tsp}";
            switch (detail.rate) {
                case ActivityConts.RateGood:
                    detailViewHolder.ItemDistrictdetailTsp.SetBackgroundResource(Resource.Drawable.tsp_good_bg);
                    break;
                case ActivityConts.RateNormal:
                    detailViewHolder.ItemDistrictdetailTsp.SetBackgroundResource(Resource.Drawable.tsp_normal_bg);
                    break;
                case ActivityConts.RateBad:
                    detailViewHolder.ItemDistrictdetailTsp.SetBackgroundResource(Resource.Drawable.tsp_bad_bg);
                    break;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var holder = new DistrictDetailAdapterViewHolder(LayoutInflater.Inflate(Resource.Layout.item_districtdetail, parent, false))
            {
                OnDistrictDetailItemClickListener = OnDistrictDetailItemClickListener,
                AdapterData = AdapterData
            };
            return holder;
        }
    }

    public class DistrictDetailAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        public TextView ItemDistrictdetailNo { get; }

        public TextView ItemDistrictdetailCountyname { get; }

        public TextView ItemDistrictdetailName { get; }

        public TextView ItemDistrictdetailTsp { get; }

        public View ItemBg { get; }

        public List<DistrictDetail> AdapterData { get; set; }

        public IOnDistrictDetailItemClickListener OnDistrictDetailItemClickListener { get; set; }

        public DistrictDetailAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public DistrictDetailAdapterViewHolder(View itemView) : base(itemView)
        {
            ItemBg = itemView.FindViewById(Resource.Id.item_districtdetail_bg);
            ItemBg.SetOnClickListener(this);
            ItemDistrictdetailNo = (TextView) itemView.FindViewById(Resource.Id.item_districtdetail_no);
            ItemDistrictdetailCountyname = (TextView) itemView.FindViewById(Resource.Id.item_districtdetail_countyname);
            ItemDistrictdetailName = (TextView) itemView.FindViewById(Resource.Id.item_districtdetail_name);
            ItemDistrictdetailTsp = (TextView) itemView.FindViewById(Resource.Id.item_districtdetail_tsp);
        }

        public void OnClick(View v)
        {
            if (v.Id != Resource.Id.item_districtdetail_bg) return;
            if (OnDistrictDetailItemClickListener == null) return;
            if(AdapterPosition < AdapterData.Count && AdapterPosition >= 0)
            {
                OnDistrictDetailItemClickListener.OnDistrictItemClick(AdapterPosition);
            }
        }
    }

    public interface IOnDistrictDetailItemClickListener
    {
        void OnDistrictItemClick(int position);
    }

    public class OnDistrictDetailItemClickListener : IOnDistrictDetailItemClickListener
    {
        private readonly Action _action;

        public OnDistrictDetailItemClickListener(Action action)
        {
            _action = action;
        }

        public void OnDistrictItemClick(int position)
        {
            _action?.Invoke();
        }
    }
}