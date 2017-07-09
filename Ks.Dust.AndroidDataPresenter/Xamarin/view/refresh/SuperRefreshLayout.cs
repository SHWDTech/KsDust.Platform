using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh
{
    public class SuperRefreshLayout : SwipeRefreshLayout, IOnLoadMoreListener, IOnCheckMoreContentListener, SwipeRefreshLayout.IOnRefreshListener, ViewTreeObserver.IOnGlobalLayoutListener
    {
        public LoadMoreRecyclerView RecyclerView { get; private set; }

        public IOnCheckMoreContentListener OnCheckMoreContentListener { get; set; }

        public IOnLoadingListener OnLoadingListener { get; set; }

        private bool CanOnlyRefreshOrLoading { get; set; }

        private bool _isLayoutFinished;

        private bool _isRefreshPending;

        public SuperRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SuperRefreshLayout(Context context) : base(context)
        {
            Init();
        }

        public SuperRefreshLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        private void Init()
        {
            ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();
            InitRecyclerView();
        }

        public bool InitRecyclerView()
        {
            CanOnlyRefreshOrLoading = true;
            FindLoadMoreListView(this);
            if (RecyclerView != null)
            {
                RecyclerView.OnLoadMoreListener = this;
                RecyclerView.OnCheckMoreContentListener = this;
                SetColorSchemeResources(Resource.Color.loading_color, Resource.Color.loading_color, Resource.Color.loading_color);
                base.SetOnRefreshListener(this);
                return true;
            }

            return false;
        }

        public override bool Enabled
        {
            get => base.Enabled && (!CanOnlyRefreshOrLoading || !IsLoading());
            set => base.Enabled = value;
        }

        public void FindLoadMoreListView(View v)
        {
            if (RecyclerView != null) return;
            if (v is LoadMoreRecyclerView)
            {
                RecyclerView = (LoadMoreRecyclerView) v;
                return;
            }
            if (v is ViewGroup)
            {
                var viewGroup = (ViewGroup)v;
                for (var i = 0; i < viewGroup.ChildCount &&RecyclerView == null; i++)
                {
                    FindLoadMoreListView(viewGroup.GetChildAt(i));
                }
            }
        }

        private bool CanRecyclerViewScrollUp()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.HoneycombMr2)
            {
                return ViewCompat.CanScrollVertically(RecyclerView, -1) || RecyclerView.ScrollY > 0;
            }
            return ViewCompat.CanScrollVertically(RecyclerView, -1);
        }

        public override bool CanChildScrollUp()
        {
            var flag = CanRecyclerViewScrollUp();
            flag = flag || RecyclerView.IsLoading();
            return flag;
        }

        public bool IsLoading()
        {
            return Refreshing || RecyclerView.IsLoading() || _isRefreshPending;
        }

        public void SetAdapter(RecyclerView.Adapter adapter)
        {
            if (RecyclerView != null)
            {
                RecyclerView.SetAdapter(adapter);
            }
            else
            {
                Log.Debug("", "MagicRefreshLayout not inflated");
            }
        }

        public void SetLayoutManager(RecyclerView.LayoutManager layoutManager)
        {
            if (RecyclerView != null)
            {
                RecyclerView.SetLayoutManager(layoutManager);
            }
            else
            {
                Log.Debug("", "MagicRefreshLayout not inflated");
            }
        }

        public void StartRefresh()
        {
            if (!IsLoading() && OnLoadingListener != null)
            {
                if (_isLayoutFinished)
                {
                    Refreshing = true;
                    OnRefresh();
                }
                else
                {
                    _isRefreshPending = true;
                }
            }
        }

        public void StopLoading()
        {
            if (RecyclerView.IsLoading())
            {
                RecyclerView.SetLoadingMore(false);
            }else if (Refreshing)
            {
                Refreshing = false;
                RecyclerView.TryLLoadMore();
            }
            _isRefreshPending = false;
        }

        public override void SetOnRefreshListener(IOnRefreshListener listener)
        {
            
        }

        public void OnLoadMore()
        {
            if (Refreshing) return;
            OnLoadingListener?.OnLoadMore();
            RecyclerView.UpdateView();
        }

        public bool CanContentLoadMore()
        {
            var flag = false;
            if (OnCheckMoreContentListener != null)
            {
                flag = OnCheckMoreContentListener.CanContentLoadMore();
            }

            return flag && ! Refreshing;
        }

        public void OnRefresh()
        {
            OnLoadingListener?.OnRefresh();
            RecyclerView.UpdateView();
        }

        public void OnGlobalLayout()
        {
            ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            _isLayoutFinished = true;
            if (_isRefreshPending)
            {
                _isRefreshPending = false;
                Refreshing = true;
                OnRefresh();
            }
        }
    }

    public interface IOnLoadingListener : SwipeRefreshLayout.IOnRefreshListener, IOnLoadMoreListener
    {

    }
}