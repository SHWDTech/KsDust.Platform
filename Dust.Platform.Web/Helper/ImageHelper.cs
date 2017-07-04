using System.Drawing;

namespace Dust.Platform.Web.Helper
{
    public static class ImageHelper
    {
        public static void GetImageThumbSize(Bitmap bitmap, out int width, out int height)
        {
            width = 128;
            height = 128;
            if (bitmap.Height == bitmap.Width) return;
            if (bitmap.Height > bitmap.Width)
            {
                width = (int) (bitmap.Width / (bitmap.Height / 128.0f));
            }
            if (bitmap.Width > bitmap.Height)
            {
                height = (int)(bitmap.Height / (bitmap.Width / 128.0f));
            }
        }
    }
}