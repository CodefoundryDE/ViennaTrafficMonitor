using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

    public class SplitFlapDisplay : ContentControl {

        private ContentPresenter _mainContent;
        private Rectangle _rectangleTop;
        private Rectangle _rectangleBottom;

        static SplitFlapDisplay() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(typeof(SplitFlapDisplay)));
        }

        public SplitFlapDisplay() {

        }

        public override void OnApplyTemplate() {
            _mainContent = Template.FindName("Part_MainContent", this) as ContentPresenter;
            _rectangleTop = Template.FindName("Part_RectangleTop", this) as Rectangle;
            _rectangleBottom = Template.FindName("Part_RectangleBottom", this) as Rectangle;
            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent) {
            string text = newContent.ToString();
            if (_mainContent != null && _rectangleTop != null && _rectangleBottom != null) {
                //_rectangleTop.Fill = _CreateBrushFromVisual(_mainContent, (int)_mainContent.ActualWidth, (int)_mainContent.ActualHeight);
                Brush brush = _CreateBrushFromVisual(_mainContent, (int)_mainContent.ActualWidth, (int)_mainContent.ActualHeight);
                _rectangleBottom.Fill = brush;

            }
            _BeginAnimateContentReplacement();
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

        private void _BeginAnimateContentReplacement() {
            ScaleTransform fallTransform = new ScaleTransform();
            SkewTransform widthTransform = new SkewTransform();

            TransformGroup group = new TransformGroup();
            group.Children.Add(fallTransform);
            group.Children.Add(widthTransform);

            widthTransform.CenterX = 0;
            widthTransform.CenterY = _rectangleBottom.ActualHeight;

            _rectangleBottom.Visibility = Visibility.Visible;
            _rectangleBottom.RenderTransform = group;

            IEasingFunction ease = new CircleEase {
                EasingMode = EasingMode.EaseInOut
            };

            fallTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _CreateAnimation(-1, 1, 0.5, ease));

            widthTransform.BeginAnimation(SkewTransform.AngleXProperty, _CreateAnimation(0, -10, 0.25, null, (s, e) => {
                widthTransform.BeginAnimation(SkewTransform.AngleXProperty, _CreateAnimation(-10, 0, 0.25, null));
            }));
        }

        private static AnimationTimeline _CreateAnimation(double from, double to, double time, IEasingFunction ease = null, EventHandler completed = null) {
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
