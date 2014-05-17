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

    public class SplitFlapPanel : AbstractAnimatedContentControl {

        private TextBlock _displayTop;
        private TextBlock _displayBottom;
        private Rectangle _rectangleTop;
        private Rectangle _rectangleBottom;

        static SplitFlapPanel() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitFlapPanel), new FrameworkPropertyMetadata(typeof(SplitFlapPanel)));
        }

        public override void OnApplyTemplate() {
            _displayTop = Template.FindName("Part_DisplayTop", this) as TextBlock;
            _displayBottom = Template.FindName("Part_DisplayBottom", this) as TextBlock;
            _rectangleTop = Template.FindName("Part_RectangleTop", this) as Rectangle;
            _rectangleBottom = Template.FindName("Part_RectangleBottom", this) as Rectangle;
            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent) {
            if (_displayTop != null && _displayBottom != null && _rectangleTop != null && _rectangleBottom != null) {
                _rectangleTop.Fill = _CreateBrushFromVisual(_displayTop, (int)_rectangleTop.ActualWidth, (int)_rectangleTop.ActualHeight);
                _BeginAnimateContentReplacement();
            }
            base.OnContentChanged(oldContent, newContent);
        }

        private void _BeginAnimateContentReplacement() {
            ScaleTransform flapTransformTop = new ScaleTransform();   
            flapTransformTop.CenterY = _rectangleTop.ActualHeight;    
            TransformGroup groupTop = new TransformGroup();           
            groupTop.Children.Add(flapTransformTop);                  
            _rectangleTop.RenderTransform = groupTop;                 


            ScaleTransform flapTransformBottom = new ScaleTransform();
            flapTransformBottom.CenterY = _rectangleBottom.ActualHeight / 2;
            TransformGroup groupBottom = new TransformGroup();
            groupBottom.Children.Add(flapTransformBottom);
            _rectangleBottom.RenderTransform = groupBottom;

            _rectangleTop.Visibility = Visibility.Visible;
            _rectangleBottom.Visibility = Visibility.Visible;

            IEasingFunction ease = new BackEase() { EasingMode = EasingMode.EaseOut };

            flapTransformTop.BeginAnimation(ScaleTransform.ScaleYProperty, _CreateAnimation(1, 0, 0.1, null, (object sender, EventArgs e) => {
                _rectangleTop.Visibility = Visibility.Hidden;
                _rectangleBottom.Visibility = Visibility.Visible;
                _rectangleBottom.Fill = _CreateBrushFromVisual(_displayBottom, (int)_rectangleBottom.ActualWidth, (int)_rectangleBottom.ActualHeight);
                flapTransformBottom.BeginAnimation(ScaleTransform.ScaleYProperty, _CreateAnimation(0, 1, 0.1, ease, (object s, EventArgs ea) => {
                    _rectangleBottom.Visibility = Visibility.Hidden;
                }));
            }));
        }

        

    }
}
