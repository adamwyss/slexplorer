using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ijv.Redstone.Explorer;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// The interaction logic for ExplorerViewSelector.xaml
    /// </summary>
    public partial class ExplorerViewSelector : Button
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the SelectedView dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedViewProperty = DependencyProperty.Register(
            "SelectedView",
            typeof(ExplorerViews),
            typeof(ExplorerViewSelector),
            new PropertyMetadata(OnSelectedViewPropertyChanged));

        /// <summary>
        /// Identifies the ScaleFactor dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            "ScaleFactor",
            typeof(double),
            typeof(ExplorerViewSelector),
            new PropertyMetadata(1.0));

        /// <summary>
        /// Occurs when the SelectedView property value changes and updates the visual state.
        /// </summary>
        /// <param name="sender">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnSelectedViewPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ExplorerViewSelector control = sender as ExplorerViewSelector;
            if (control != null)
            {
                control.ClearValue(ScaleFactorProperty);

                bool contentUpdated = false;

                object value = e.NewValue;
                if (value != null)
                {
                    string key = value.ToString();

                    if (control.Resources.Contains(key))
                    {
                        control.Content = control.Resources[key] as BitmapSource;
                        contentUpdated = true;
                    }
                }

                if (!contentUpdated)
                {
                    control.ClearValue(SelectedViewProperty);
                }
            }
        }

        #endregion

        /// <summary>
        /// The button displays the menu and provides the options.
        /// </summary>
        private Button buttonElement;

        /// <summary>
        /// The popup control that hosts the menu.
        /// </summary>
        private Popup popupElement;

        /// <summary>
        /// The menu that contains the list of views and the selector.
        /// </summary>
        private FrameworkElement popupElementChild;

        /// <summary>
        /// The dismiss canvas that detects the dismiss click for the popup.
        /// </summary>
        private Canvas popupDismissCanvas;

        /// <summary>
        /// Initializes a new instance of the ExplorerViewButton class.
        /// </summary>
        public ExplorerViewSelector()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the currently selected view type.
        /// </summary>
        public ExplorerViews SelectedView
        {
            get { return (ExplorerViews)this.GetValue(SelectedViewProperty); }
            set { this.SetValue(SelectedViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected scale of the icons.  scale 1.0 = 16x16, 2.0 = 32x32, 3.0 = 64x64, etc
        /// </summary>
        public double ScaleFactor
        {
            get { return (double)this.GetValue(ScaleFactorProperty); }
            set { this.SetValue(ScaleFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the menu is visible.
        /// </summary>
        protected bool IsPopupOpen
        {
            get
            {
                if (this.popupElement != null)
                {
                    return this.popupElement.IsOpen;
                }
                return false;
            }

            set
            {
                if (this.popupElement != null)
                {
                    this.popupElement.IsOpen = value;
                }
            }
        }

        /// <summary>
        /// Applies the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.buttonElement != null)
            {
                this.buttonElement.Click -= this.OnOptionsButtonClicked;
            }
            this.buttonElement = this.GetTemplateChild("PART_OptionsButton") as Button;
            if (this.buttonElement != null)
            {
                this.buttonElement.Click += this.OnOptionsButtonClicked;
            }

            if (this.popupElement != null)
            {
                this.popupElement.Child = this.popupElementChild;
            }
            if (this.popupElementChild != null)
            {
                this.popupElementChild.SizeChanged += this.OnPopupSizeChanged;
                this.popupElement.Child = null;
            }
            if (this.popupDismissCanvas != null)
            {
                this.popupDismissCanvas.MouseLeftButtonDown -= this.OnMouseButtonDownToDismiss;
                this.popupDismissCanvas.MouseRightButtonDown -= this.OnMouseButtonDownToDismiss;
                this.popupDismissCanvas = null;
            }
            this.popupElement = this.GetTemplateChild("PART_OptionsPopup") as Popup;
            if (this.popupElement != null)
            {
                this.popupElementChild = this.popupElement.Child as FrameworkElement;
                if (this.popupElementChild != null)
                {
                    this.popupElementChild.SizeChanged += this.OnPopupSizeChanged;
                }

                this.popupDismissCanvas = new Canvas();
                this.popupDismissCanvas.Background = new SolidColorBrush(Colors.Transparent);
                //this.popupDismissCanvas.Background = new SolidColorBrush(Color.FromArgb(064, 255, 000, 255));
                this.popupDismissCanvas.MouseLeftButtonDown += this.OnMouseButtonDownToDismiss;
                this.popupDismissCanvas.MouseRightButtonDown += this.OnMouseButtonDownToDismiss;

                Canvas popupCanvas = new Canvas();
                this.popupElement.Child = popupCanvas;
                popupCanvas.Children.Add(this.popupDismissCanvas);
                popupCanvas.Children.Add(this.popupElementChild);
            }

            if (this.thumbElement != null)
            {
                this.thumbElement.DragStarted -= this.OnThumbDragStarted;
                this.thumbElement.DragDelta -= this.OnThumbDragDelta;
                this.thumbElement.DragCompleted -= this.OnThumbDragCompleted;
            }
            this.thumbElement = this.GetTemplateChild("PART_Thumb") as Thumb;
            if (this.thumbElement != null)
            {
                this.thumbElement.DragStarted += this.OnThumbDragStarted;
                this.thumbElement.DragDelta += this.OnThumbDragDelta;
                this.thumbElement.DragCompleted += this.OnThumbDragCompleted;
            }


            Type enumerationType = typeof(ExplorerViews);
            List<FieldInfo> results = new List<FieldInfo>();
            if (enumerationType.IsEnum)
            {
                foreach (FieldInfo field in enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (field.IsLiteral)
                    {
                        results.Add(field);
                    }
                }
            }

            foreach (FieldInfo enumField in results)
            {
                ExplorerViews enumValue = (ExplorerViews)enumField.GetValue(typeof(ExplorerViews));
                string xamlName = "PART_Button" + enumField.Name;

                Button button = this.GetTemplateChild(xamlName) as Button;
                if (button != null)
                {
                    button.Click += delegate { this.SelectedView = enumValue; this.IsPopupOpen = false; };
                    this.lookup.Add(enumValue, button);
                }
            }
        }

        double dragoffset = 0.0;

        private void OnThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            this.dragoffset = e.VerticalOffset;
        }

        private Dictionary<ExplorerViews, double> GetLocations(Grid track)
        {
            Dictionary<ExplorerViews, double> location = new Dictionary<ExplorerViews, double>();

            Type enumerationType = typeof(ExplorerViews);
            List<ExplorerViews> results = new List<ExplorerViews>();
            if (enumerationType.IsEnum)
            {
                foreach (FieldInfo field in enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (field.IsLiteral)
                    {
                        results.Add((ExplorerViews)field.GetValue(enumerationType));
                    }
                }
            }

            foreach (ExplorerViews view in results)
            {
                Button button;
                bool success = this.lookup.TryGetValue(view, out button);

                Point pt = button.TransformToVisual(track).Transform(new Point(0, 0));
                double centerY = pt.Y + (button.ActualHeight / 2.0);

                location.Add(view, centerY);
            }

            location.OrderByDescending(d => d.Value);

            return location;
        }

        private void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            this.dragoffset += e.VerticalChange;

            Grid track = this.thumbElement.Parent as Grid;

            if (track != null && track.RowDefinitions.Count == 3)
            {
                RowDefinition definition = track.RowDefinitions[0];

                Dictionary<ExplorerViews, double> location = this.GetLocations(track);

                KeyValuePair<ExplorerViews, double> closestEntry = new KeyValuePair<ExplorerViews, double>(this.SelectedView, location[this.SelectedView]);

                double thumbOffset = Math.Max(0, Math.Min(track.ActualHeight, this.dragoffset));

                bool handled = false;

                List<ExplorerViews> list1 = location.Keys.ToList();
                int index = list1.IndexOf(this.SelectedView);

                double centerOfThumb = thumbOffset + this.thumbElement.ActualHeight / 2.0;

                bool isCurrentViewScalable = this.IsScalableView(this.SelectedView);
                if (e.VerticalChange < 0 && isCurrentViewScalable && index > 0)
                {

                    double maxScalableVerticle = location[list1[index - 1]] + 10;
                    
                    double verticalPositionOfCurrentSelection = location[list1[index]];

                    if (thumbOffset < closestEntry.Value - this.thumbElement.ActualHeight / 2.0

                        && index > 0 &&

                        maxScalableVerticle < centerOfThumb)
                    {
                        double maxScale = verticalPositionOfCurrentSelection - maxScalableVerticle;
                        double position = verticalPositionOfCurrentSelection - centerOfThumb;

                        // the maximum scale should be unattainable, because then it would be the next
                        // item.
                        maxScale += 1;

                        this.ScaleFactor = 1.0 + Math.Round(position / maxScale, 1);

                        definition.Height = new GridLength(thumbOffset);
                        handled = true;
                    }

                }

                if (!handled)
                {

                    foreach (KeyValuePair<ExplorerViews, double> entry in location)
                    {
                        double v1 = Math.Abs(closestEntry.Value - thumbOffset);
                        double v2 = Math.Abs(entry.Value - thumbOffset);

                        if (v2 < v1)
                        {
                            closestEntry = entry;
                        }
                    }

                    // we are close enough to move it.
                    if (closestEntry.Key != this.SelectedView)
                    {
                        double spot = closestEntry.Value - this.thumbElement.ActualHeight / 2.0;
                        definition.Height = new GridLength(spot);

                        this.SelectedView = closestEntry.Key;
                    }
                }
            }

        }

        private void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.IsPopupOpen = false;
        }

        private Thumb thumbElement;
        private Dictionary<ExplorerViews, Button> lookup = new Dictionary<ExplorerViews, Button>();

        /// <summary>
        /// Increment to the next view when the button is clicked.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            switch (this.SelectedView)
            {
                case ExplorerViews.ExtraLargeIcons:
                case ExplorerViews.LargeIcons:
                case ExplorerViews.MediumIcons:
                case ExplorerViews.SmallIcons:
                    this.SelectedView = ExplorerViews.List;
                    break;

                case ExplorerViews.List:
                    this.SelectedView = ExplorerViews.Details;
                    break;

                case ExplorerViews.Details:
                    this.SelectedView = ExplorerViews.Tiles;
                    break;

                case ExplorerViews.Tiles:
                    this.SelectedView = ExplorerViews.Content;
                    break;

                case ExplorerViews.Content:
                    this.SelectedView = ExplorerViews.LargeIcons;
                    break;
            }
        }

        /// <summary>
        /// Returns true if the specified view supports scaling.
        /// </summary>
        /// <param name="view">The explorer view that we are checking.</param>
        private bool IsScalableView(ExplorerViews view)
        {
            return view == ExplorerViews.LargeIcons ||
                   view == ExplorerViews.MediumIcons ||
                   view == ExplorerViews.SmallIcons;
        }


        private void UpdateTrackLayout()
        {
            Grid track = this.thumbElement.Parent as Grid;

            if (track != null && track.RowDefinitions.Count == 3)
            {
                RowDefinition definition = track.RowDefinitions[0];

                // get the location of the current button
                Button button;
                bool found = this.lookup.TryGetValue(this.SelectedView, out button);

                this.thumbElement.Visibility = found ? Visibility.Visible : Visibility.Collapsed;

                if (found)
                {
                    Point pt = button.TransformToVisual(track).Transform(new Point(0, 0));

                    double centerY = pt.Y + (button.ActualHeight / 2.0);

                    double spot = centerY - this.thumbElement.ActualHeight / 2.0;

                    definition.Height = new GridLength(spot);
                }

            }
        }

        /// <summary>
        /// Shows the popup when the option button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnOptionsButtonClicked(object sender, RoutedEventArgs e)
        {
            this.IsPopupOpen = true;
        }

        /// <summary>
        /// Position the popup in the client area after it has appeared (or moved).
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The SizeChangedEventArgs that contains the event arguments.</param>
        private void OnPopupSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.popupElement != null && this.popupElementChild != null && this.IsPopupOpen)
            {
                this.UpdateTrackLayout();

                double hostWidth = Application.Current.Host.Content.ActualWidth;
                double hostHeight = Application.Current.Host.Content.ActualHeight;

                GeneralTransform transform = this.TransformToVisual(null);
                // find the position of ourselves on the screen
                Point upperLeft = transform.Transform(new Point(0, 0));
                Point lowerRight = transform.Transform(new Point(this.ActualWidth, this.ActualHeight));

                double distanceToBottomEdge = hostHeight - lowerRight.Y;
                double distanceToTopEdge = upperLeft.Y;
                if (distanceToBottomEdge < this.popupElementChild.ActualHeight &&
                    distanceToTopEdge > distanceToBottomEdge)
                {
                    // the popup will not fit below this control and will better fit above
                    // it, so we will place it there.
                    this.popupElement.VerticalOffset = -this.popupElementChild.ActualHeight;
                }
                else
                {
                    // place the popup directly below the control
                    this.popupElement.VerticalOffset = this.ActualHeight;
                }

                // adjust the popup horizontal placement so it doesn't disappear off the right edge.
                double distanceToRightEdge = hostWidth - upperLeft.X;
                if (distanceToRightEdge < this.popupElementChild.ActualWidth)
                {
                    this.popupElement.HorizontalOffset = this.ActualWidth - this.popupElementChild.ActualWidth;
                }

                // position the dismiss click canvas to cover the entire screen.
                Matrix m = Matrix.Identity;
                m.OffsetX = -upperLeft.X - this.popupElement.HorizontalOffset;
                m.OffsetY = -upperLeft.Y - this.popupElement.VerticalOffset;
                MatrixTransform transform2 = new MatrixTransform();
                transform2.Matrix = m;
                this.popupDismissCanvas.RenderTransform = transform2;
                this.popupDismissCanvas.Width = hostWidth;
                this.popupDismissCanvas.Height = hostHeight;
            }
        }

        /// <summary>
        /// Hides the popup when the dismiss canvas is clicked on.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnMouseButtonDownToDismiss(object sender, MouseButtonEventArgs e)
        {
            this.IsPopupOpen = false;
        }

    }
}
