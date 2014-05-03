using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VtmFramework.View {

    public class AnimatedContentControl : ContentControl {

        private ContentPresenter _mainContent;
        private Shape _paintArea;

        public Transform OldContentTransform { get; set; }
        public Transform NewContentTransform { get; set; }

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
                _paintArea.Fill = _CreateBrushFromVisual(_mainContent, (int)this.ActualWidth, (int)this.ActualHeight);
                _BeginAnimateContentReplacement();
            }
            base.OnContentChanged(oldContent, newContent);
        }

        /// <summary>
        /// Erzeugt einen gerenderten Brush aus einem Visual
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Brush _CreateBrushFromVisual(Visual v, int width, int height) {
            if (v == null)
                throw new ArgumentNullException("v");
            RenderTargetBitmap target = new RenderTargetBitmap(Math.Max(width, 1), Math.Max(height, 1), 96, 96, PixelFormats.Pbgra32);
            target.Render(v);
            Brush brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Führt die Animation für beide Contents aus
        /// </summary>
        private void _BeginAnimateContentReplacement() {
            OldContentTransform = new TranslateTransform();
            NewContentTransform = new TranslateTransform();

            _paintArea.Visibility = Visibility.Visible;
            _paintArea.RenderTransform = OldContentTransform;
            _mainContent.RenderTransform = NewContentTransform;

            NewContentTransform.BeginAnimation(TranslateTransform.XProperty, _CreateAnimation(this.ActualWidth, 0));
            OldContentTransform.BeginAnimation(TranslateTransform.XProperty, _CreateAnimation(0, -this.ActualWidth, (s, e) => {
                _paintArea.Visibility = Visibility.Hidden;
            }));
        }

        /// <summary>
        /// Erstellt eine Animation
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        private static AnimationTimeline _CreateAnimation(double from, double to, EventHandler completed = null) {
            IEasingFunction ease = new BackEase {
                Amplitude = 0.5,
                EasingMode = EasingMode.EaseInOut
            };
            Duration duration = new Duration(TimeSpan.FromSeconds(1));
            var animation = new DoubleAnimation(from, to, duration);
            animation.EasingFunction = ease;
            if (completed != null) {
                animation.Completed += completed;
            }
            animation.Freeze();
            return animation;
        }

    }
}
