using Android.Support.V7.Widget;
using Android.Views;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view
{
    public class SearchViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        private readonly SearchDialogItemClickListener _clickListener;

        public SearchViewHolder(View itemView, SearchAdapter adapter) : base(itemView)
        {
            _clickListener = adapter.ItemClickListener;
            itemView.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            _clickListener?.OnSearchItemClick(AdapterPosition);
        }
    }
}