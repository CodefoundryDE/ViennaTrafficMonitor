using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace VtmFramework.Library {

    public static class AnimateLib {

        /// <summary>
        /// Erzeugt einen gerenderten Brush aus einem Visual
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static Brush CreateBrushFromVisual(Visual visual, int width, int height) {
            if (visual == null)
                throw new ArgumentNullException("visual");
            RenderTargetBitmap target = new RenderTargetBitmap(Math.Max(width, 1), Math.Max(height, 1), 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            ImageBrush brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Erstellt eine Animation.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="ease"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static DoubleAnimation CreateAnimation(double from, double to, double beginTime, double time, IEasingFunction ease = null, EventHandler completed = null) {
            DoubleAnimation animation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromSeconds(time)));
            animation.BeginTime = TimeSpan.FromSeconds(beginTime);
            if (ease != null) animation.EasingFunction = ease;
            if (completed != null) animation.Completed += completed;
            animation.Freeze();
            return animation;
        }

    }
}
