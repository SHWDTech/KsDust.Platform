using System.Collections.Generic;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view
{
    public class SearchDialogItemClickListener : IOnSearchItemClick
    {
        private readonly List<SearchResult> _searchResults;

        private readonly IOnSearchClickListener _clickListener;

        public SearchDialogItemClickListener(SearchDialog searchDialog)
        {
            _searchResults = searchDialog.SearchResults;
            _clickListener = searchDialog.SearchCllickListener;
        }

        public void OnSearchItemClick(int position)
        {
            if (position < _searchResults.Count)
            {
                var searchResult = _searchResults[position];
                _clickListener.OnSearchClick(searchResult);
            }
        }
    }
}