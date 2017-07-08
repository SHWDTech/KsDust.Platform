using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.adapter
{
    public class SearchAdapter : AbstractRecyclerAdapter<SearchResult>
    {
        public SearchAdapter(Context context) : base(context)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is SearchViewHolder)
            {
                
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _layoutInflater.Inflate(Resource.Layout.item_search, parent, false);
            return new SearchViewHolder(view);
        }

        class SearchViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            private TextView _nameTextView;
            private TextView _levelTextView;

            public SearchViewHolder(View itemView) : base(itemView)
            {
                itemView.SetOnClickListener(this);
                _nameTextView = (TextView)itemView.FindViewById(Resource.Id.searchName);
                _levelTextView = (TextView)itemView.FindViewById(Resource.Id.searchLevel);
            }

            public void OnClick(View v)
            {
                Console.WriteLine("get in");
            }
        }
    }
}