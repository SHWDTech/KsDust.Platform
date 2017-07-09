using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh
{
    public class FooterView : LinearLayout
    {
        public const int FooterStateNoMoreData = 10;

        public const int FooterStateCLickToLoad = 11;

        public const int FooterStateLoading = 12;

        private ProgressBar _progressBar;

        private TextView _textView;

        private View _rootView;

        private int _footerState = FooterStateNoMoreData;

        public OnClickLoadMoreListener OnClickLoadMoreListener { get; set; }

        private readonly IEmptyDetector _emptyDetector;

        public FooterView(Context context, IEmptyDetector detector) : this(context)
        {
            _emptyDetector = detector;
            InitView(context);
        }

        public FooterView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FooterView(Context context) : base(context)
        {
        }

        public FooterView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitView(context);
        }

        public FooterView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitView(context);
        }

        public FooterView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        private void InitView(Context context)
        {
            var v = Inflate(context, Resource.Layout.footerview_layout, this);
            _rootView = v.FindViewById(Resource.Id.footerview_root_loadmore);
            _progressBar = (ProgressBar)v.FindViewById(Resource.Id.footerview_pb);
            _textView = (TextView)v.FindViewById(Resource.Id.footerview_more_tv);

            using (var clickListener = new ClickListener(() =>
            {
                if (_footerState == FooterStateCLickToLoad)
                {
                    OnClickLoadMoreListener?.OnClickLoadMore();
                }
            }))
            {
                _rootView.SetOnClickListener(clickListener);
            }

            _rootView.Clickable = false;
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);
        }

        public void UpdateFooterView()
        {
            if (_emptyDetector != null)
            {
                if (_emptyDetector.IsEmpty())
                {
                    HideFooter();
                }
                else
                {
                    ShowFooter();
                }
            }
            switch (_footerState)
            {
                case FooterStateNoMoreData:
                    _progressBar.Visibility = ViewStates.Gone;
                    _textView.Text = string.Empty;
                    _rootView.Clickable = false;
                    break;
                case FooterStateCLickToLoad:
                    _progressBar.Visibility = ViewStates.Gone;
                    _textView.Text = string.Empty;
                    _rootView.Clickable = true;
                    break;
                case FooterStateLoading:
                    _progressBar.Visibility = ViewStates.Visible;
                    _textView.Text = string.Empty;
                    _rootView.Clickable = false;
                    break;
            }
        }

        private void HideFooter()
        {
            _rootView.Visibility = ViewStates.Gone;
        }

        private void ShowFooter()
        {
            _rootView.Visibility = ViewStates.Visible;
        }

        public void SetFooterState(int state)
        {
            _footerState = state;
            UpdateFooterView();
        }
    }

    public interface IOnClickLoadMoreListener
    {
        void OnClickLoadMore();
    }

    public class OnClickLoadMoreListener : IOnClickLoadMoreListener
    {
        private readonly Action _action;

        public OnClickLoadMoreListener(Action action)
        {
            _action = action;
        }

        public void OnClickLoadMore()
        {
            _action.Invoke();
        }
    }
}