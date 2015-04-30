using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VtmFramework.Library;

namespace VtmFramework.View {

    public class WindowAnimationControl : ContentControl {

        private ContentPresenter _mainContent;
        private Shape _paintArea;

        public Transform OldContentTransform { get; set; }
        public Transform NewContentTransform { get; set; }

        static WindowAnimationControl() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowAnimationControl), new FrameworkPropertyMetadata(typeof(WindowAnimationControl)));
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
                _paintArea.Fill = AnimateLib.CreateBrushFromVisual(_mainContent, (int)this.ActualWidth, (int)this.ActualHeight);
                _BeginAnimateContentReplacement();
            }
            base.OnContentChanged(oldContent, newContent);
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

            IEasingFunction ease = new BackEase {
                Amplitude = 0.5,
                EasingMode = EasingMode.EaseInOut
            };

            NewContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimateLib.CreateAnimation(this.ActualWidth, 0, 0, 1, ease));
            OldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimateLib.CreateAnimation(0, -this.ActualWidth, 0, 1, ease, (s, e) => {
                _paintArea.Visibility = Visibility.Hidden;
            }));
        }

    }
}
