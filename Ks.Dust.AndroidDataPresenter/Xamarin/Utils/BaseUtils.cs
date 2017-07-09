using Android.Content;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.Utils
{
    public class BaseUtils
    {
        public static int Dip2Px(Context context, float dipValue)
        {
            var scale = context.Resources.DisplayMetrics.Density;
            return (int)(dipValue * scale + 0.5f);
        }
    }
}