using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VtmFramework.Library;

namespace VtmFramework.View
{

    public class SplitFlapPanel : ContentControl
    {

        private TextBlock _displayTop;
        private TextBlock _displayBottom;
        private Rectangle _rectangleTop;
        private Rectangle _rectangleBottom;

        private Rectangle _rectangleBottomStatic;

        static SplitFlapPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitFlapPanel), new FrameworkPropertyMetadata(typeof(SplitFlapPanel)));
        }

        public override void OnApplyTemplate()
        {
            _displayTop = Template.FindName("Part_DisplayTop", this) as TextBlock;
            _displayBottom = Template.FindName("Part_DisplayBottom", this) as TextBlock;
            _rectangleTop = Template.FindName("Part_RectangleTop", this) as Rectangle;
            _rectangleBottom = Template.FindName("Part_RectangleBottom", this) as Rectangle;

            _rectangleBottomStatic = Template.FindName("Part_RectangleBottomStatic", this) as Rectangle;

            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (_displayTop != null && _displayBottom != null && _rectangleTop != null && _rectangleBottom != null)
            {
                _rectangleTop.Fill = AnimateLib.CreateBrushFromVisual(_displayTop, (int)_rectangleTop.ActualWidth, (int)_rectangleTop.ActualHeight);
                _rectangleBottomStatic.Fill = AnimateLib.CreateBrushFromVisual(_displayBottom, (int)_rectangleBottomStatic.ActualWidth, (int)_rectangleBottomStatic.ActualHeight);
                _BeginAnimateContentReplacement();
            }
            base.OnContentChanged(oldContent, newContent);
        }

        private void _BeginAnimateContentReplacement()
        {
            var flapTransformTop = new ScaleTransform { CenterY = _rectangleTop.ActualHeight / 2 };
            _rectangleTop.RenderTransform = flapTransformTop;

            var flapTransformBottom = new ScaleTransform { CenterY = _rectangleBottom.ActualHeight / 2 };
            _rectangleBottom.RenderTransform = flapTransformBottom;

            _rectangleTop.Visibility = Visibility.Visible;
            _rectangleBottom.Visibility = Visibility.Visible;
            _rectangleBottomStatic.Visibility = Visibility.Visible;

            IEasingFunction ease = null;//new BackEase() { EasingMode = EasingMode.EaseOut };

            AnimationTimeline animationTop = AnimateLib.CreateAnimation(1, 0, 0, 0.1, null, (object s, EventArgs e) =>
            {
                _rectangleTop.Visibility = Visibility.Hidden;
                _rectangleBottom.Visibility = Visibility.Visible;
                _rectangleBottom.Fill = AnimateLib.CreateBrushFromVisual(_displayBottom, (int)_rectangleBottom.ActualWidth, (int)_rectangleBottom.ActualHeight);
            });

            AnimationTimeline animationBottom = AnimateLib.CreateAnimation(0, 1, 0.1, 0.1, ease, (object s, EventArgs e) =>
            {
                _rectangleBottom.Visibility = Visibility.Hidden;
                _rectangleBottomStatic.Visibility = Visibility.Hidden;
            });

            flapTransformTop.BeginAnimation(ScaleTransform.ScaleYProperty, animationTop, HandoffBehavior.SnapshotAndReplace);
            flapTransformBottom.BeginAnimation(ScaleTransform.ScaleYProperty, animationBottom, HandoffBehavior.SnapshotAndReplace);
        }

    }
}
