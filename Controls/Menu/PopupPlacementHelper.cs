using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Helper class for determining the placement of the popup control.
    /// Shamelessly stolen from System.Windows.Controls.Tooltip in Silverlight 3 disassembly.
    /// </summary>
    internal static class PopupPlacementHelper
    {
        /// <summary />
        internal static Point[] GetTranslatedPoints(FrameworkElement element)
        {
            Point[] pointArray = new Point[4];
            if (element != null)
            {
                GeneralTransform transform = element.TransformToVisual(null);
                pointArray[0] = transform.Transform(new Point(0.0, 0.0));
                pointArray[1] = transform.Transform(new Point(element.ActualWidth, 0.0));
                pointArray[1].X--;
                pointArray[2] = transform.Transform(new Point(0.0, element.ActualHeight));
                pointArray[2].Y--;
                pointArray[3] = transform.Transform(new Point(element.ActualWidth, element.ActualHeight));
                pointArray[3].X--;
                pointArray[3].Y--;
            }
            return pointArray;
        }

        /// <summary />
        internal static Rect GetBounds(params Point[] interestPoints)
        {
            double num2;
            double num4;
            double x = num2 = interestPoints[0].X;
            double y = num4 = interestPoints[0].Y;
            for (int i = 1; i < interestPoints.Length; i++)
            {
                double num6 = interestPoints[i].X;
                double num7 = interestPoints[i].Y;
                if (num6 < x)
                {
                    x = num6;
                }
                if (num6 > num2)
                {
                    num2 = num6;
                }
                if (num7 < y)
                {
                    y = num7;
                }
                if (num7 > num4)
                {
                    num4 = num7;
                }
            }
            return new Rect(x, y, (num2 - x) + 1.0, (num4 - y) + 1.0);
        }

        /// <summary />
        internal static Point PlacePopup(Rect plugin, Point[] target, Point[] toolTip, PlacementMode placement)
        {
            Point[] pointArray;
            double y = 0.0;
            double x = 0.0;
            Rect bounds = GetBounds(target);
            Rect rect2 = GetBounds(toolTip);
            double width = rect2.Width;
            double height = rect2.Height;
            if (placement == PlacementMode.Right)
            {
                double num5 = Math.Max((double)0.0, (double)(target[0].X - 1.0));
                double num6 = plugin.Width - Math.Min(plugin.Width, target[1].X + 1.0);
                if ((num6 < width) && (num6 < num5))
                {
                    placement = PlacementMode.Left;
                }
            }
            else if (placement == PlacementMode.Left)
            {
                double num7 = Math.Min(plugin.Width, target[1].X + width) - target[1].X;
                double num8 = target[0].X - Math.Max((double)0.0, (double)(target[0].X - width));
                if ((num8 < width) && (num8 < num7))
                {
                    placement = PlacementMode.Right;
                }
            }
            else if (placement == PlacementMode.Top)
            {
                double num9 = target[0].Y - Math.Max((double)0.0, (double)(target[0].Y - height));
                double num10 = Math.Min(plugin.Height, plugin.Height - height) - target[2].Y;
                if ((num9 < height) && (num9 < num10))
                {
                    placement = PlacementMode.Bottom;
                }
            }
            else if (placement == PlacementMode.Bottom)
            {
                double num11 = Math.Max(0.0, target[0].Y);
                double num12 = plugin.Height - Math.Min(plugin.Height, target[2].Y);
                if ((num12 < height) && (num12 < num11))
                {
                    placement = PlacementMode.Top;
                }
            }
            switch (placement)
            {
                case PlacementMode.Bottom:
                    pointArray = new Point[] { new Point(target[2].X, Math.Max((double)0.0, (double)(target[2].Y + 1.0))), new Point((target[3].X - width) + 1.0, Math.Max((double)0.0, (double)(target[2].Y + 1.0))), new Point(0.0, Math.Max((double)0.0, (double)(target[2].Y + 1.0))) };
                    break;

                case PlacementMode.Right:
                    pointArray = new Point[] { new Point(Math.Max((double)0.0, (double)(target[1].X + 1.0)), target[1].Y), new Point(Math.Max((double)0.0, (double)(target[3].X + 1.0)), (target[3].Y - height) + 1.0), new Point(Math.Max((double)0.0, (double)(target[1].X + 1.0)), 0.0) };
                    break;

                case PlacementMode.Left:
                    pointArray = new Point[] { new Point(Math.Min(plugin.Width, target[0].X) - width, target[1].Y), new Point(Math.Min(plugin.Width, target[2].X) - width, (target[3].Y - height) + 1.0), new Point(Math.Min(plugin.Width, target[0].X) - width, 0.0) };
                    break;

                case PlacementMode.Top:
                    pointArray = new Point[] { new Point(target[0].X, Math.Min(target[0].Y, plugin.Height) - height), new Point((target[1].X - width) + 1.0, Math.Min(target[0].Y, plugin.Height) - height), new Point(0.0, Math.Min(target[0].Y, plugin.Height) - height) };
                    break;

                default:
                    pointArray = new Point[0];
                    break;
            }
            double num13 = width * height;
            int index = -1;
            double num15 = 0.0;
            for (int i = 0; i < pointArray.Length; i++)
            {
                Rect rect3 = new Rect(pointArray[i].X, pointArray[i].Y, width, height);
                rect3.Intersect(plugin);
                double d = rect3.Width * rect3.Height;
                if (double.IsInfinity(d))
                {
                    index = pointArray.Length - 1;
                    break;
                }
                if (d > num15)
                {
                    index = i;
                    num15 = d;
                }
                if (d == num13)
                {
                    index = i;
                    break;
                }
            }
            x = pointArray[index].X;
            y = pointArray[index].Y;
            if (index > 1)
            {
                if ((placement == PlacementMode.Left) || (placement == PlacementMode.Right))
                {
                    if (((y != target[0].Y) && (y != target[1].Y)) && (((y + height) != target[0].Y) && ((y + height) != target[1].Y)))
                    {
                        double num18 = bounds.Top + (bounds.Height / 2.0);
                        if ((num18 > 0.0) && ((num18 - 0.0) > (plugin.Height - num18)))
                        {
                            y = plugin.Height - height;
                        }
                        else
                        {
                            y = 0.0;
                        }
                    }
                }
                else if (((placement == PlacementMode.Top) || (placement == PlacementMode.Bottom)) && (((x != target[0].X) && (x != target[1].X)) && (((x + width) != target[0].X) && ((x + width) != target[1].X))))
                {
                    double num19 = bounds.Left + (bounds.Width / 2.0);
                    if ((num19 > 0.0) && ((num19 - 0.0) > (plugin.Width - num19)))
                    {
                        x = plugin.Width - width;
                    }
                    else
                    {
                        x = 0.0;
                    }
                }
            }
            return new Point(x, y);
        }
    }
}
