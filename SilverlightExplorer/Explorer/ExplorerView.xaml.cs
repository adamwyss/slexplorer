using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Ijv.Redstone.Controls;

namespace Ijv.Redstone.Explorer.Views
{
    /// <summary />
    public partial class ExplorerView : UserControl
    {
        /// <summary />
        public ExplorerView()
        {
            this.InitializeComponent();

            this.Loaded += this.WhenControlLoadedInitializeBindings;

            // selection code
            this.ItemsSurface.AddHandler(
                MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(this.SelectionTarget_MouseLeftButtonDown),
                true);

            this.ItemsSurface.AddHandler(
                MouseLeftButtonUpEvent,
                new MouseButtonEventHandler(this.SelectionTarget_MouseLeftButtonUp),
                true);

            this.ItemsSurface.MouseMove += new MouseEventHandler(SelectionTarget_MouseMove);
            this.ItemsSurface.LostMouseCapture += new MouseEventHandler(ItemsSurface_LostMouseCapture);
        }

        /// <summary />
        private void WhenControlLoadedInitializeBindings(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.WhenControlLoadedInitializeBindings;

            // binds properties to the grid definination (not supported in silverlight)
            new GridDefinitionBindingHelper(this.HACK_BINDING_NavigationColumnWidth,
                                          this.HACK_BINDING_NavigationColumnSplitter,
                                          this.HACK_BINDING_NavigationColumnGridSplitter,
                                          (ExplorerViewModel)this.DataContext,
                                          ExplorerViewModel.NavigationColumnWidthProperty,
                                          ExplorerViewModel.IsNavigationVisibleProperty);

            new GridDefinitionBindingHelper(this.HACK_BINDING_DetailsRowHeight,
                                          this.HACK_BINDING_DetailsRowSplitter,
                                          this.HACK_BINDING_DetailsRowGridSplitter,
                                          (ExplorerViewModel)this.DataContext,
                                          ExplorerViewModel.DetailsRowHeightProperty,
                                          ExplorerViewModel.IsDetailsVisibleProperty);

            new GridDefinitionBindingHelper(this.HACK_BINDING_PreviewColumnWidth,
                                          this.HACK_BINDING_PreviewColumnSplitter,
                                          this.HACK_BINDING_PreviewColumnGridSplitter,
                                          (ExplorerViewModel)this.DataContext,
                                          ExplorerViewModel.PreviewColumnWidthProperty,
                                          ExplorerViewModel.IsPreviewVisibleProperty);
        }

        #region Selection Stuff

        private const double JitterThreshold = 15;
        Point startPos;
        bool leftMouseButtonDown = false;
        bool selectionActive = false;

        void SelectionTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.leftMouseButtonDown = this.ItemsSurface.CaptureMouse();

            if (this.leftMouseButtonDown)
            {
                this.startPos = e.GetPosition(this.ItemsSurface);
            }
        }

        void SelectionTarget_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ItemsSurface.ReleaseMouseCapture();
        }

        void SelectionTarget_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.leftMouseButtonDown)
            {
                Point currentPos = e.GetPosition(this.ItemsSurface);
                double deltaX = this.startPos.X - currentPos.X;
                double deltaY = this.startPos.Y - currentPos.Y;

                if (!this.selectionActive && (Math.Abs(deltaX) > JitterThreshold || Math.Abs(deltaY) > JitterThreshold))
                {
                    this.selectionActive = true;
                }

                if (this.selectionActive)
                {
                    Rect bounds = new Rect(
                        deltaX > 0 ? this.startPos.X - deltaX : this.startPos.X,
                        deltaY > 0 ? this.startPos.Y - deltaY : this.startPos.Y,
                        Math.Abs(deltaX) + 1,
                        Math.Abs(deltaY) + 1);


                    // adjust to constrain to the bounds of the control
                    {
                        if (bounds.Top < 0)
                        {
                            bounds.Height += bounds.Top;
                            bounds.Y = 0;
                        }
                        if (bounds.Left < 0)
                        {
                            bounds.Width += bounds.Left;
                            bounds.X = 0;
                        }
                        if (bounds.Bottom > this.ItemsSurface.ActualHeight)
                        {
                            bounds.Height = this.ItemsSurface.ActualHeight - bounds.Top;
                        }
                        if (bounds.Right > this.ItemsSurface.ActualWidth)
                        {
                            bounds.Width = this.ItemsSurface.ActualWidth - bounds.Left;
                        }
                    }

                    this.UpdateSelectionRectangle(this.SelectionRectangle, true, bounds);
                }
            }
        }

        void ItemsSurface_LostMouseCapture(object sender, MouseEventArgs e)
        {
            this.UpdateSelectionRectangle(this.SelectionRectangle, false, Rect.Empty);
            this.leftMouseButtonDown = false;
            this.selectionActive = false;
        }

        private void UpdateSelectionRectangle(Rectangle visualElement, bool visible, Rect rect)
        {
            // show / hide
            visualElement.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            if (visible)
            {
                Canvas.SetTop(visualElement, rect.Top);
                Canvas.SetLeft(visualElement, rect.Left);
                visualElement.Width = rect.Width;
                visualElement.Height = rect.Height;
            }
        }

        #endregion
    }


}
