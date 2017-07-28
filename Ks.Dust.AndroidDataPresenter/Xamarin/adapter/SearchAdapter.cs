using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ApplicationConcept;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Ks.Dust.AndroidDataPresenter.Xamarin.view;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    public class SearchAdapter : AbstractRecyclerAdapter<SearchResult>
    {
        public RecyclerView RecyclerView { get; private set; }

        public SearchDialogItemClickListener ItemClickListener { get; set; }

        private TextView _nameTextView;

        private TextView _levelTextView;

        public SearchAdapter(Context context, List<SearchResult> adapterData) : base(context, adapterData)
        {

        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as SearchViewHolder;
            if (viewHolder == null) return;
            var searchResult = AdapterData[position];
            _nameTextView.Text = searchResult.objectName;
            var objectType = (ObjectType)searchResult.objectLevel;
            switch (objectType)
            {
                case ObjectType.Project:
                    _levelTextView.Text = "工程";
                    break;
                case ObjectType.District:
                    _levelTextView.Text = "区县";
                    break;
                case ObjectType.Device:
                    _levelTextView.Text = "设备";
                    break;
                case ObjectType.Enterprise:
                    _levelTextView.Text = "施工单位";
                    break;
                default:
                    _levelTextView.Text = "未知";
                    break;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.Inflate(Resource.Layout.item_search, parent, false);
            _nameTextView = (TextView)itemView.FindViewById(Resource.Id.searchName);
            _levelTextView = (TextView)itemView.FindViewById(Resource.Id.searchLevel);
            return new SearchViewHolder(itemView, this);
        }

        public void SetRecyclerView(RecyclerView view)
        {
            view.SetAdapter(this);
            RecyclerView = view;
        }
    }
}