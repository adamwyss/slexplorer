using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace Ijv.Redstone.Controls
{
    /// <summary />
    public partial class MenuItem
    {
        /// <summary>
        /// The submenu that shows this items contents.
        /// </summary>
        private Menu submenu;

        /// <summary>
        /// Timer used to delay the appearance of the submenu.  By introducing a 
        /// slight delay, it allows the user to skip over it with out flicking
        /// the submenu.  Matches windows menu functionality.
        /// </summary>
        private DispatcherTimer submenuOpenDelayTimer;

        /// <summary>
        /// Timer used to delay the closing of the sub menu.  Allows the user
        /// to leave the bounds of the menu item and not immediately close the 
        /// menu - ie, they can cut corners and quickly get to menu items in
        /// a long list.  Matches windows menu functionality.
        /// </summary>
        private DispatcherTimer submenuCloseDelayTimer;

        /// <summary>
        /// True if eventhandlers are hooked up to the menu and submenu watching
        /// for dismiss actions; otherwise false.
        /// 
        /// This property shouldn't need to exist if we correctly hook and unhook
        /// the events when we are spose to.
        /// </summary>
        private bool dismissNotificationHooked;

        /// <summary>
        /// Ensure that the submenu and associated items are created and initialized.
        /// </summary>
        private void EnsureSubmenu()
        {
            if (this.submenu == null)
            {
                Menu parentMenu = this.CoerseParentMenu();
                this.submenu = new Menu(parentMenu);
                this.submenu.Placement = PlacementMode.Right;
                this.submenu.PlacementTarget = this;
                this.submenu.Margin = new Thickness(-2, -2, 0, 0);

                // hook to the menu closed event so we can unhook our event handlers when it closes.
                this.submenu.Closed += delegate(object sender, RoutedEventArgs e) { this.UnhookMenuForDismissNotification(); };

                // do we want to do this everytime?
                this.submenu.ItemsSource = this.Items;
            }

            if (this.submenuOpenDelayTimer == null)
            {
                this.submenuOpenDelayTimer = new DispatcherTimer();
                this.submenuOpenDelayTimer.Interval = TimeSpan.FromMilliseconds(175);
                this.submenuOpenDelayTimer.Tick += this.OnOpenDelayElasped;
            }

            if (this.submenuCloseDelayTimer == null)
            {
                this.submenuCloseDelayTimer = new DispatcherTimer();
                this.submenuCloseDelayTimer.Interval = TimeSpan.FromMilliseconds(175);
                this.submenuCloseDelayTimer.Tick += this.OnCloseDelayElasped;
            }
        }

        /// <summary>
        /// Displays a sub menu from the current menu item.
        /// </summary>
        private void OpenSubmenu()
        {
            this.EnsureSubmenu();

            // DEV NOTE: the two timer classes are created in the ensuresubmenu method.

            if (this.submenuCloseDelayTimer.IsEnabled)
            {
                // a close request is in progress, so we need to cancel and unhook our dismiss handlers
                this.submenuCloseDelayTimer.Stop();
                this.UnhookMenuForDismissNotification();
            }

            if (!this.submenuOpenDelayTimer.IsEnabled)
            {
                this.submenuOpenDelayTimer.Start();
                this.HookMenuForDismissNotification();
            }
        }

        /// <summary>
        /// Hides the currently displayed submenu.
        /// </summary>
        private void CloseSubmenu()
        {
            if (this.submenuOpenDelayTimer != null && this.submenuOpenDelayTimer.IsEnabled)
            {
                // an open request is in progress, so we will cancel and unhook the handlers.
                this.submenuOpenDelayTimer.Stop();
                this.UnhookMenuForDismissNotification();
            }

            // if a close request isn't already in progress start a close request.
            if (this.submenuCloseDelayTimer != null && !this.submenuCloseDelayTimer.IsEnabled)
            {
                this.submenuCloseDelayTimer.Start();
                // the menu.closed event will fire and unhook our dismiss notification handlers (unless the close action was canceled)
            }
        }

        /// <summary>
        /// Hook this menu's parent and watch for dismiss actions.
        /// </summary>
        private void HookMenuForDismissNotification()
        {
            if (!this.dismissNotificationHooked)
            {
                this.dismissNotificationHooked = true;

                this.submenu.MouseMove += this.OnSubmenuMouseMove;

                Menu menu = this.CoerseParentMenu();
                if (menu != null)
                {
                    menu.MouseMove += this.OnParentMenuMouseMove;
                }
            }
        }

        /// <summary>
        /// Unhook this menu's parent.
        /// </summary>
        private void UnhookMenuForDismissNotification()
        {
            if (this.dismissNotificationHooked)
            {
                this.dismissNotificationHooked = false;

                this.submenu.MouseMove -= this.OnSubmenuMouseMove;

                Menu menu = this.CoerseParentMenu();
                if (menu != null)
                {
                    menu.MouseMove -= this.OnParentMenuMouseMove;
                }
            }
        }

        /// <summary>
        /// Examines this menu item and finds the menu that is directly hosting it.
        /// </summary>
        /// <returns>The menu that contains this menu item.</returns>
        private Menu CoerseParentMenu()
        {
            Menu menu = this.Parent as Menu;
            if (menu != null)
            {
                return menu;
            }

            MenuItem menuItem = this.Parent as MenuItem;
            if (menuItem != null)
            {
                return menuItem.submenu;
            }

            throw new NotImplementedException("Unable to coerse the parent menu from this object.");
        }

        /// <summary>
        /// Opens the timer after a slight delay.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The EventArgs that contains the event data.</param>
        private void OnOpenDelayElasped(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
            }

            this.submenu.IsOpen = true;
        }

        /// <summary>
        /// Closes the timer after a slight delay.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The EventArgs that contains the event data.</param>
        private void OnCloseDelayElasped(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
            }

            this.submenu.IsOpen = false;
        }

        /// <summary>
        /// Dismisses the menu if the mouse is not directly over this menu item.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The MouseEventArgs that contains the event data.</param>
        private void OnParentMenuMouseMove(object sender, MouseEventArgs e)
        {
            this.EnsureSubmenu();

            Menu menu = (Menu)sender;
            Rect menuBounds = new Rect(0, 0, menu.ActualWidth, menu.ActualHeight);
            Point menuMouseCoords = e.GetPosition(menu);

            Rect menuItemBounds = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            Point menuItemMouseCoords = e.GetPosition(this);

            // if the mouse is over the parent menu and not over the current menu item, we will close the submenu.
            if (!this.submenuCloseDelayTimer.IsEnabled &&
                menuBounds.Contains(menuMouseCoords) && !menuItemBounds.Contains(menuItemMouseCoords))
            {
                this.CloseSubmenu();
            }
        }

        /// <summary>
        /// Prevents the submenu from being dismissed if the mouse is over it.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The MouseEventArgs that contains the event data.</param>
        private void OnSubmenuMouseMove(object sender, MouseEventArgs e)
        {
            if (this.submenuCloseDelayTimer.IsEnabled)
            {
                this.submenuCloseDelayTimer.Stop();
                this.UnhookMenuForDismissNotification();
            }
        }
    }
}
