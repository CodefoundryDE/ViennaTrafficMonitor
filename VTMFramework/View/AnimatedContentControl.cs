using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VtmFramework.View {

    public class AnimatedContentControl : ContentControl {

        private ContentPresenter _mainContent;
        private Shape _paintArea;

        static AnimatedContentControl() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }

        /// <summary>
        /// Wird aufgerufen wenn das Template geladen wurde.
        /// </summary>
        public override void OnApplyTemplate() {
            _mainContent = Template.FindName("Part_MainContent", this) as ContentPresenter;
            _paintArea = Template.FindName("Part_PaintArea", this) as Shape;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Wird aufgerufen wenn sich der Inhalt des Controls ändert.
        /// </summary>
        /// <param name="oldContent"></param>
        /// <param name="newContent"></param>
        protected override void OnContentChanged(object oldContent, object newContent) {
            if (_paintArea != null && _mainContent != null) {
                _paintArea.Fill = CreateBrushFromVisual(_mainContent);
                BeginAnimateContentReplacement();
            }
            base.OnContentChanged(oldContent, newContent);
        }

        /// <summary>
        /// Erzeugt einen gerenderten Brush aus einem Visual
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Brush CreateBrushFromVisual(Visual v) {
            if (v == null)
                throw new ArgumentNullException("v");
            RenderTargetBitmap target = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(v);
            Brush brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Führt die Animation für beide Contents aus
        /// </summary>
        private void BeginAnimateContentReplacement() {
            TranslateTransform newContentTransform = new TranslateTransform();
            TranslateTransform oldContentTransform = new TranslateTransform();

            _paintArea.RenderTransform = oldContentTransform;
            _mainContent.RenderTransform = newContentTransform;
            _paintArea.Visibility = Visibility.Visible;

            newContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(this.ActualWidth, 0));
            oldContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(0, -this.ActualWidth, (s, e) => {
                _paintArea.Visibility = Visibility.Hidden;
            }));
        }

        /// <summary>
        /// Erstellt eine Animation
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="whenDone"></param>
        /// <returns></returns>
        private AnimationTimeline CreateAnimation(double from, double to, EventHandler whenDone = null) {
            IEasingFunction ease = new BackEase {
                Amplitude = 0.5,
                EasingMode = EasingMode.EaseOut
            };
            Duration duration = new Duration(TimeSpan.FromSeconds(3));
            var animation = new DoubleAnimation(from, to, duration);
            animation.EasingFunction = ease;
            if (whenDone != null) {
                animation.Completed += whenDone;
            }
            animation.Freeze();
            return animation;
        }

    }
}
