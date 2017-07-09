using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ApplicationConcept;
using Java.Lang;
using Ks.Dust.AndroidDataPresenter.Xamarin.adapter;
using Ks.Dust.AndroidDataPresenter.Xamarin.component;
using Ks.Dust.AndroidDataPresenter.Xamarin.Model;
using Newtonsoft.Json;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view
{
    public class SearchDialog : Dialog, View.IOnClickListener, IDialogInterfaceOnDismissListener, ITextWatcher, TextView.IOnEditorActionListener
    {
        public EditText EditText { get; private set; }

        public RecyclerView RecyclerView { get; private set; }

        public List<SearchResult> SearchResults { get; } = new List<SearchResult>();

        public SearchAdapter SearchAdapter { get; private set; }

        public View ClearView { get; private set; }

        public IOnSearchClickListener SearchCllickListener { get; private set; }

        public SearchDialog(Context context, bool cancelable, EventHandler cancelHandler) : base(context, cancelable, cancelHandler)
        {
        }

        public SearchDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        
        public SearchDialog(Context context, bool cancelable, IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener)
        {
        }

        public SearchDialog(Context context, int themeResId) : base(context, themeResId)
        {
            
        }
        public SearchDialog(Context context) : this(context, Resource.Style.chat_dialog)
        {
            InitView(context);
        }

        private void InitView(Context context)
        {
            SetContentView(View.Inflate(context, Resource.Layout.dialog_search, null));

            Window.Attributes.Width = ViewGroup.LayoutParams.MatchParent;
            Window.Attributes.Height = ViewGroup.LayoutParams.MatchParent;
            Window.Attributes.SoftInputMode = SoftInput.AdjustResize;
            SetOnDismissListener(this);

            ClearView = FindViewById(Resource.Id.search_clear);
            ClearView.SetOnClickListener(this);
            EditText = (EditText) FindViewById(Resource.Id.edit);
            EditText.AddTextChangedListener(this);
            EditText.SetOnEditorActionListener(this);
            
            RecyclerView = (RecyclerView) FindViewById(Resource.Id.recyclerView);
            RecyclerView.AddItemDecoration(new DividerItemDecoration(context, DividerItemDecoration.Vertical));
            RecyclerView.SetLayoutManager(new LinearLayoutManager(context.ApplicationContext));
            RecyclerView.AddOnScrollListener(new SearchDialogOnScrollListener(this));
        }

        public void SetOnClickListener(IOnSearchClickListener clickListener)
        {
            SearchCllickListener = clickListener;
            SearchAdapter = new SearchAdapter(Context, SearchResults)
            {
                ItemClickListener = new SearchDialogItemClickListener(this)
            };
            SearchAdapter.SetRecyclerView(RecyclerView);
        }

        public void OnClick(View v)
        {
            EditText.Text = string.Empty;
            ClearView.Visibility = ViewStates.Gone;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            EditText.Text = string.Empty;
            SearchResults.Clear();
            SearchAdapter.NotifyDataSetChanged();
        }

        private ICharSequence _currentText;

        private int _selectionStart;

        private int _selectionEnd;

        private int _maxNum = 200;

        private int _beforeCount;

        public void AfterTextChanged(IEditable s)
        {
            try
            {
                var counter = CountCharLength(_currentText.ToString());
                ClearView.Visibility = counter > 0 ? ViewStates.Visible : ViewStates.Gone;
                _selectionStart = EditText.SelectionStart;
                _selectionEnd = EditText.SelectionEnd;
                if (counter <= _maxNum) return;
                s.Delete(_selectionStart - 1, _selectionEnd);
                EditText.Text = s.ToString();
                SetSelectionToEnd();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            if (string.IsNullOrWhiteSpace(_currentText.ToString()))
            {
                _beforeCount = CountCharLength(_currentText.ToString());
            }
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            _currentText = s;
        }

        private static int CountCharLength(string text)
        {
            if ((string.IsNullOrWhiteSpace(text)))
            {
                return 0;
            }
            var length = text.Length;
            var count = length;
            for (var i = 0; i < length; i++)
            {
                if (!IsAscii(text[i])) count++;
            }

            return count;
        }

        private static bool IsAscii(char ch)
        {
            return ch > 0x00 && ch < 0x7F;
        }

        private void SetSelectionToEnd()
        {
            var editable = EditText.EditableText;
            Selection.SetSelection(editable, editable.Length());
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId != ImeAction.Search) return false;
            var searchText = EditText.Text;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Toast.MakeText(v.Context, "请输入搜索内容", ToastLength.Short).Show();
                return true;
            }

            var handler = new HttpResponseHandler();
            handler.OnResponse += args =>
            {
                SearchResults.Clear();
                SearchResults.AddRange(JsonConvert.DeserializeObject<List<SearchResult>>(args.Response));
                SearchAdapter.NotifyDataSetChanged();
            };
            ApiManager.GetSearch(searchText, AuthticationManager.Instance.AccessToken, handler);

            return false;
        }
    }
}