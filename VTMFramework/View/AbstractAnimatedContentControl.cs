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

namespace VtmFramework.View {
    public abstract class AbstractAnimatedContentControl : ContentControl {

        /// <summary>
        /// Erzeugt einen gerenderten Brush aus einem Visual
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected static Brush _CreateBrushFromVisual(Visual v, int width, int height) {
            if (v == null)
                throw new ArgumentNullException("v");
            RenderTargetBitmap target = new RenderTargetBitmap(Math.Max(width, 1), Math.Max(height, 1), 96, 96, PixelFormats.Pbgra32);
            target.Render(v);
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
        protected static DoubleAnimation _CreateAnimation(double from, double to, double time, IEasingFunction ease = null, EventHandler completed = null) {
            Duration duration = new Duration(TimeSpan.FromSeconds(time));
            var animation = new DoubleAnimation(from, to, duration);

            if (ease != null) {
                animation.EasingFunction = ease;
            }
            if (completed != null) {
                animation.Completed += completed;
            }
            animation.Freeze();
            return animation;
        }

    }
}
