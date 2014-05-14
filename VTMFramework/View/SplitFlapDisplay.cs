﻿using System;
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

        private TextBlock _displayTop;
        private ContentPresenter _displayBottom;
        private Rectangle _rectangleTop;
        private Rectangle _rectangleBottom;

        static SplitFlapDisplay() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(typeof(SplitFlapDisplay)));
        }

        public override void OnApplyTemplate() {
            _displayTop = Template.FindName("Part_DisplayTop", this) as TextBlock;
            _displayBottom = Template.FindName("Part_DisplayBottom", this) as ContentPresenter;
            _rectangleTop = Template.FindName("Part_RectangleTop", this) as Rectangle;
            _rectangleBottom = Template.FindName("Part_RectangleBottom", this) as Rectangle;
            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent) {
            string text = newContent.ToString();
            if (_displayTop != null && _displayBottom != null && _rectangleTop != null && _rectangleBottom != null) {
                _rectangleTop.Fill = _CreateBrushFromVisual(_displayTop, (int)_rectangleTop.ActualWidth, (int)_rectangleTop.ActualHeight);
                _rectangleBottom.Fill = _CreateBrushFromVisual(_displayBottom, (int)_rectangleBottom.ActualWidth, (int)_rectangleBottom.ActualHeight);
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
            brush.Opacity = 1;
            brush.Freeze();
            return brush;
        }

        private void _BeginAnimateContentReplacement() {
            ScaleTransform flapTransformTop = new ScaleTransform();
            flapTransformTop.CenterY = _rectangleTop.ActualHeight;
            //SkewTransform widthTransform = new SkewTransform();

            ScaleTransform flapTransformBottom = new ScaleTransform();

            TransformGroup groupTop = new TransformGroup();
            groupTop.Children.Add(flapTransformTop);
            //group.Children.Add(widthTransform);

            TransformGroup groupBottom = new TransformGroup();
            groupBottom.Children.Add(flapTransformBottom);

            //widthTransform.CenterX = 0;
            //widthTransform.CenterY = _rectangleBottom.ActualHeight;

            _rectangleTop.Visibility = Visibility.Visible;
            _rectangleTop.RenderTransform = groupTop;

            _rectangleBottom.RenderTransform = groupBottom;

            /*IEasingFunction ease = new CircleEase {
                EasingMode = EasingMode.EaseInOut
            };*/

            flapTransformTop.BeginAnimation(ScaleTransform.ScaleYProperty, _CreateFlapAnimation(1, 0, 0.25, (object sender, EventArgs e) => {
                _rectangleTop.Visibility = Visibility.Hidden;
                _rectangleBottom.Visibility = Visibility.Visible;
                flapTransformBottom.BeginAnimation(ScaleTransform.ScaleYProperty, _CreateFlapAnimation(0, 1, 0.25, (object s, EventArgs ea) => {
                    _rectangleBottom.Visibility = Visibility.Hidden;
                }));
            }));

            // widthTransform.BeginAnimation(SkewTransform.AngleXProperty, _CreateFlapAnimation(0, -10, 0.25, (s, e) => {
            //     widthTransform.BeginAnimation(SkewTransform.AngleXProperty, _CreateFlapAnimation(-10, 0, 0.25));
            // }));
        }

        private static AnimationTimeline _CreateFlapAnimation(double from, double to, double time, EventHandler completed = null) {
            Duration duration = new Duration(TimeSpan.FromSeconds(time));
            var animation = new DoubleAnimation(from, to, duration);
            // Todo: Easing
            if (completed != null) {
                animation.Completed += completed;
            }
            animation.Freeze();
            return animation;
        }

    }
}
