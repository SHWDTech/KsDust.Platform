using System.Linq;
using Android.Support.V7.Widget;
using Android.Widget;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh
{
    public class RecyclerViewOnScroll : RecyclerView.OnScrollListener
    {
        public const int DefaultThreshold = 5;

        private readonly LoadMoreRecyclerView _recyclerView;

        private int _currentScrollState;

        private int _lastTotalItemCount;

        private int _totalItemCount;

        private bool _notCalledSinceLLlastTotalChanged;

        public int VisibleThreshold { get; set; }= DefaultThreshold;

        public RecyclerViewOnScroll(LoadMoreRecyclerView recyclerView)
        {
            _recyclerView = recyclerView;
        }

        private bool NeedLoadMore(int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            if (_lastTotalItemCount != totalItemCount)
            {
                _lastTotalItemCount = totalItemCount;
                _notCalledSinceLLlastTotalChanged = true;
            }
            var loadMore = !_recyclerView.IsLoading() && _notCalledSinceLLlastTotalChanged &&
                            firstVisibleItem + visibleItemCount + VisibleThreshold >= totalItemCount;

            return loadMore;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            if (recyclerView != _recyclerView) return;
            TryDetectNeedLoadMore(false);
        }

        public void TryDetectNeedLoadMore(bool force)
        {
            var recyclerView = _recyclerView;
            var lastVisibleItem = 0;
            var firstVisibleItem = 0;
            var layoutManager = recyclerView.GetLayoutManager();
            _totalItemCount = layoutManager.ItemCount;
            if (layoutManager is GridLayoutManager)
            {
                var gridLayoutManager = ((GridLayoutManager)layoutManager);
                lastVisibleItem = gridLayoutManager.FindLastVisibleItemPosition();
                firstVisibleItem = gridLayoutManager.FindLastVisibleItemPosition();
            }else if (layoutManager is LinearLayoutManager)
            {
                var linearLayoutManager = ((LinearLayoutManager)layoutManager);
                lastVisibleItem = linearLayoutManager.FindLastVisibleItemPosition();
                firstVisibleItem = linearLayoutManager.FindLastVisibleItemPosition();
            }
            else if (layoutManager is StaggeredGridLayoutManager) {
                var staggeredGridLayoutManager = ((StaggeredGridLayoutManager)layoutManager);
                var lastPositions = new int[((StaggeredGridLayoutManager)layoutManager).SpanCount];
                staggeredGridLayoutManager.FindLastVisibleItemPositions(lastPositions);
                lastVisibleItem = FindMax(lastPositions);
                firstVisibleItem = staggeredGridLayoutManager.FindLastVisibleItemPositions(lastPositions)[0];
            }
            if (!NeedLoadMore(firstVisibleItem, lastVisibleItem - firstVisibleItem + 1, _totalItemCount) ||
                (!force && _currentScrollState == (int) ScrollState.Idle)) return;
            if (_recyclerView.PerformLoadMore())
            {
                _notCalledSinceLLlastTotalChanged = false;
            }
        }

        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            if (newState == (int)ScrollState.Idle)
            {
                recyclerView.Invalidate(); // FIXME to much load
            }
            _currentScrollState = newState;
        }

        private int FindMax(int[] lastPosition)
        {
            var max = lastPosition[0];
            return lastPosition.Concat(new[] {max}).Max();
        }
    }
}