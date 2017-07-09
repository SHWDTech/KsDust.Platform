using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Java.Lang;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh
{
    public class LoadMoreRecyclerView : RecyclerView, IRunnable, IEmptyDetector
    {
        private FooterView _footerView;

        public IOnLoadMoreListener OnLoadMoreListener { get; set; }

        public IOnCheckMoreContentListener OnCheckMoreContentListener { get; set; }

        private RecyclerViewOnScroll _onScrollListener;

        private volatile bool _isLoading;

        public LoadMoreRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public LoadMoreRecyclerView(Context context) : base(context)
        {
            Init(context);
        }

        public LoadMoreRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public LoadMoreRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        public void Init(Context context)
        {
            _footerView = new FooterView(context, this)
            {
                OnClickLoadMoreListener = new OnClickLoadMoreListener(() =>
                {
                    SetLoadingMore(true);
                })
            };
            _onScrollListener = new RecyclerViewOnScroll(this);
            AddOnScrollListener(_onScrollListener);
        }

        public override void SetAdapter(Adapter adapter)
        {
            if (adapter != null)
            {
                if (adapter is WrapperBaseAdapter)
                {
                    base.SetAdapter(adapter);
                }
                else
                {
                    var warpperAdapter = new WrapperBaseAdapter(adapter);
                    warpperAdapter.AddFooter(_footerView);
                    base.SetAdapter(adapter);
                }
            }
            else
            {
                base.SetAdapter(null);
            }
            UpdateView();
        }

        public bool IsLoading() => _isLoading;

        public void Run()
        {
            if (CanContentLoadMore())
            {
                _onScrollListener.TryDetectNeedLoadMore(true);
            }
            UpdateView();
        }

        public bool IsEmpty()
        {
            var adapter = GetAdapter();
            if (adapter == null) return true;
            adapter = (adapter as WrapperBaseAdapter)?.BaseAdapter;
            if (adapter == null) return true;
            if (adapter.ItemCount == 0) return true;
            return false;
        }

        public void SetLoadingThreshold(int threshold)
        {
            _onScrollListener.VisibleThreshold = threshold;
            if (CanContentLoadMore())
            {
                _onScrollListener.TryDetectNeedLoadMore(true);
            }
        }

        public void SetLoadingMore(bool loading)
        {
            if (loading)
            {
                if (_isLoading) return;
                PerformLoadMore();
            }
            else
            {
                if (!_isLoading) return;
                PerformStopLoad();
            }
        }

        public bool PerformStopLoad()
        {
            if (!_isLoading) return false;
            _isLoading = false;
            if (CanContentLoadMore())
            {
                Post(this);
            }
            else
            {
                UpdateView();
            }
            return true;
        }

        public bool PerformLoadMore()
        {
            if (_isLoading || OnLoadMoreListener == null) return false;
            if ((CanContentLoadMore())) return false;
            _isLoading = true;
            UpdateView();
            OnLoadMoreListener.OnLoadMore();
            return true;
        }

        public void TryLLoadMore()
        {
            if (CanContentLoadMore())
            {
                Post(this);
            }
            else
            {
                UpdateView();
            }
        }

        private bool CanContentLoadMore()
        {
            return OnCheckMoreContentListener == null || OnCheckMoreContentListener.CanContentLoadMore();
        }

        public void UpdateView()
        {
            if (_isLoading)
            {
                _footerView.SetFooterState(FooterView.FooterStateLoading);
            }
            else
            {
                _footerView.SetFooterState(CanContentLoadMore()
                    ? FooterView.FooterStateCLickToLoad
                    : FooterView.FooterStateNoMoreData);
            }
        }
    }

    public interface IOnLoadMoreListener
    {
        void OnLoadMore();
    }

    public class OnLoadMoreListener : IOnLoadMoreListener
    {
        private readonly Action _action;

        public OnLoadMoreListener(Action action)
        {
            _action = action;
        }

        public void OnLoadMore()
        {
            _action?.Invoke();
        }
    }

    public interface IOnCheckMoreContentListener
    {
        bool CanContentLoadMore();
    }

    public class OnCheckMoreContentListener : IOnCheckMoreContentListener
    {
        private readonly Func<object, bool> _action;

        public OnCheckMoreContentListener(Func<object, bool> action)
        {
            _action = action;
        }

        public bool CanContentLoadMore()
        {
            return _action?.Invoke(null) == true;
        }
    }
}