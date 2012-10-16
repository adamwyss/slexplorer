using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ijv.Redstone.Design;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Allows for binding between a bindable property and a column / row defination since silverlight does not
    /// allow this.
    /// </summary>
    internal class GridDefinitionBindingHelper
    {
        private static readonly GridLength GridLengthZero = new GridLength(0);

        private double cachedDefMin = double.NaN;

        private ColumnDefinition contentColDef;
        private ColumnDefinition splitColDef;

        private RowDefinition contentRowDef;
        private RowDefinition splitRowDef;

        private GridSplitter splitter;

        private BindableObject bindableObject;
        private BindableProperty sizeProperty;
        private BindableProperty visibleProperty;

        /// <summary>
        /// Creates an instance of the GridDefinitionBindingHack class.
        /// </summary>
        private GridDefinitionBindingHelper(GridSplitter s, BindableObject o, BindableProperty p, BindableProperty p2)
        {
            // preconditions

            Argument.IsNotNull("s", s);
            Argument.IsNotNull("o", o);
            Argument.IsNotNull("p", p);

            // implementation

            this.splitter = s;
            this.bindableObject = o;
            this.sizeProperty = p;
            this.visibleProperty = p2;

            // update the property if the column width/row height changes.
            s.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
            {
                this.AdjustPropertyValue();
            };

            // update the column width/row height if the property value changes.
            o.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == this.sizeProperty.Name ||
                    (this.visibleProperty != null && e.PropertyName == this.visibleProperty.Name))
                {
                    this.AdjustDefinationValue();
                }
            };
        }

        /// <summary>
        /// Creates an instance of the GridDefinitionBindingHack class.
        /// </summary>
        public GridDefinitionBindingHelper(ColumnDefinition c, ColumnDefinition cs, GridSplitter s, BindableObject o, BindableProperty p, BindableProperty p2)
            : this(s, o, p, p2)
        {
            // preconditions

            Argument.IsNotNull("c", c);
            Argument.IsNotNull("cs", cs);

            // implementation

            this.contentColDef = c;
            this.splitColDef = cs;

            this.cachedDefMin = c.MinWidth;
            
            this.AdjustDefinationValue();
        }

        /// <summary>
        /// Creates an instance of the GridDefinitionBindingHack class.
        /// </summary>
        public GridDefinitionBindingHelper(RowDefinition r, RowDefinition rs, GridSplitter s, BindableObject o, BindableProperty p, BindableProperty p2)
            : this(s, o, p, p2)
        {
            // preconditions

            Argument.IsNotNull("r", r);
            Argument.IsNotNull("rs", rs);

            // implementation

            this.contentRowDef = r;
            this.splitRowDef = rs;

            this.cachedDefMin = r.MinHeight;

            this.AdjustDefinationValue();
        }

        /// <summary>
        /// Syncs the value of the row / column definition with the property.
        /// </summary>
        private void AdjustDefinationValue()
        {
            bool visible = true;

            if (this.visibleProperty != null)
            {
                visible = this.bindableObject.GetValue<bool>(this.visibleProperty);
            }

            if (visible)
            {
                double v = this.bindableObject.GetValue<double>(this.sizeProperty);
                GridLength value = !double.IsNaN(v) ? new GridLength(v) : GridLength.Auto;

                if (this.contentColDef != null)
                {
                    this.contentColDef.MinWidth = this.cachedDefMin;
                    this.contentColDef.Width = value;
                    this.splitColDef.Width = GridLength.Auto;
                }
                else if (this.contentRowDef != null)
                {
                    this.contentRowDef.MinHeight = this.cachedDefMin;
                    this.contentRowDef.Height = value;
                    this.splitRowDef.Height = GridLength.Auto;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                if (this.contentColDef != null)
                {
                    this.contentColDef.MinWidth = 0;
                    this.contentColDef.Width = GridLengthZero;
                    this.splitColDef.Width = GridLengthZero;
                }
                else if (this.contentRowDef != null)
                {
                    this.contentRowDef.MinHeight = 0;
                    this.contentRowDef.Height = GridLengthZero;
                    this.splitRowDef.Height = GridLengthZero;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Syncs the value of the property with the row / column definition.
        /// </summary>
        private void AdjustPropertyValue()
        {
            GridLength value;

            if (this.contentColDef != null)
            {
                value = this.contentColDef.Width;
            }
            else if (this.contentRowDef != null)
            {
                value = this.contentRowDef.Height;
            }
            else
            {
                throw new InvalidOperationException();
            }

            double v = !value.IsAuto ? value.Value : double.NaN;

            this.bindableObject.SetValue<double>(this.sizeProperty, v);
        }
    }
}
