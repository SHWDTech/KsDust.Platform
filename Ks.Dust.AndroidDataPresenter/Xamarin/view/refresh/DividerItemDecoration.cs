using System.Security;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Java.Lang;
using Android.Support.V4.View;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.view.refresh
{
    public class DividerItemDecoration : RecyclerView.ItemDecoration
    {
        private static readonly int[] AttRs = {Android.Resource.Attribute.ListDivider};

        public static readonly int Horizontal = LinearLayoutManager.Horizontal;

        public static readonly int Vertical = LinearLayoutManager.Vertical;

        private Drawable _divider;

        public int Width { get; set; }

        public int Height { get; set; }

        public int Orientation { get; private set; }

        public DividerItemDecoration(Context context, int orientation) : this(context, orientation, false)
        {
            
        }

        public DividerItemDecoration(Context context, int orientation, bool isTransparent)
        {
            var array = context.ObtainStyledAttributes(AttRs);
            _divider = array.GetDrawable(0);
            if (_divider != null && isTransparent)
            {
                _divider.SetAlpha(0);
            }
            array.Recycle();
            SetOrientation(orientation);
        }

        public DividerItemDecoration(Context context, int orientation, Drawable dividerDrawable)
        {
            _divider = dividerDrawable;
            SetOrientation(orientation);
        }

        private void SetOrientation(int orientation)
        {
            if (orientation != Horizontal && orientation != Vertical)
            {
                throw new IllegalArgumentException("invalid orientation");
            }

            Orientation = orientation;
        }

        public void DrawVertical(Canvas canvas, RecyclerView parent)
        {
            var left = parent.PaddingLeft;
            var right = parent.Width - parent.PaddingRight;
            var childCount = parent.ChildCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = parent.GetChildAt(i);
                var layoutParams = (RecyclerView.LayoutParams) child.LayoutParameters;
                var top = child.Bottom + layoutParams.BottomMargin + Math.Round(ViewCompat.GetTranslationY(child));
                var bottom = top + GetDividerHeight();
                _divider.SetBounds(left, top, right, bottom);
                _divider.Draw(canvas);
            }
        }

        public void DrawHorizontal(Canvas canvas, RecyclerView parent)
        {
            var top = parent.PaddingTop;
            var bottom = parent.Height - parent.PaddingBottom;
            var childCount = parent.ChildCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = parent.GetChildAt(i);
                var layoutParams = (RecyclerView.LayoutParams)child.LayoutParameters;
                var left = child.Right + layoutParams.RightMargin + Math.Round(ViewCompat.GetTranslationX(child));
                var right = left + GetDividerWidth();
                _divider.SetBounds(left, top, right, bottom);
                _divider.Draw(canvas);
            }
        }

        private int GetDividerWidth()
        {
            return Width > 0 ? Width : _divider.IntrinsicWidth;
        }
        private int GetDividerHeight()
        {
            return Height > 0 ? Height : _divider.IntrinsicWidth;
        }

    }
}