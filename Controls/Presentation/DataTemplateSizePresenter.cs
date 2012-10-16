using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a control with a single piece of content that changes its own template based on size.
    /// </summary>
    [ContentProperty("BoundedTemplates")]
    public class DataTemplateSizePresenter : ContentControl
    {
        /// <summary>
        /// Identifies the MinSize attached property.
        /// </summary>
        public static readonly DependencyProperty MinSizeProperty = DependencyProperty.RegisterAttached(
            "MinSize",
            typeof(Size),
            typeof(DataTemplateSizePresenter),
            new PropertyMetadata(Size.Empty));

        /// <summary>
        /// Identifies the MaxSize attached property.
        /// </summary>
        public static readonly DependencyProperty MaxSizeProperty = DependencyProperty.RegisterAttached(
            "MaxSize",
            typeof(Size),
            typeof(DataTemplateSizePresenter),
            new PropertyMetadata(Size.Empty));

        /// <summary>
        /// Identifies the BoundedTemplates attached property.
        /// </summary>
        public static readonly DependencyProperty BoundedTemplatesProperty = DependencyProperty.Register(
            "BoundedTemplates",
            typeof(DataTemplateCollection),
            typeof(DataTemplateSizePresenter),
            null);

        #region Command Get/Set Methods

        /// <summary />
        public static Size GetMaxSize(DataTemplate template)
        {
            // preconditions

            Argument.IsNotNull("template", template);

            // implementation

            return (Size)template.GetValue(MaxSizeProperty);
        }

        /// <summary />
        public static void SetMaxSize(DataTemplate template, Size value)
        {
            // preconditions

            Argument.IsNotNull("template", template);

            // implementation

            template.SetValue(MaxSizeProperty, value);
        }

        /// <summary />
        public static Size GetMinSize(DataTemplate template)
        {
            // preconditions

            Argument.IsNotNull("template", template);

            // implementation

            return (Size)template.GetValue(MinSizeProperty);
        }

        /// <summary />
        public static void SetMinSize(DataTemplate template, Size value)
        {
            // preconditions

            Argument.IsNotNull("template", template);

            // implementation

            template.SetValue(MinSizeProperty, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplateSizePresenter" /> class.
        /// </summary>
        public DataTemplateSizePresenter()
        {
            this.BoundedTemplates = new DataTemplateCollection();

            this.SizeChanged += OnControlSizeChanged;
        }

        /// <summary />
        public DataTemplateCollection BoundedTemplates
        {
            get { return (DataTemplateCollection)this.GetValue(BoundedTemplatesProperty); }
            set { this.SetValue(BoundedTemplatesProperty, value); }
        }

        /// <summary>
        /// Updates the selected data template when the control size changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs" /> that contain the event data.</param>
        private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DataTemplate selected = null;

            foreach (DataTemplate template in this.BoundedTemplates)
            {
                Size minSize = (Size)GetMinSize(template);
                Size maxSize = (Size)GetMaxSize(template);

                bool minAccepted = minSize.IsEmpty | (minSize.Height >= e.NewSize.Height && minSize.Width >= e.NewSize.Width);
                bool maxAccepted = maxSize.IsEmpty | (e.NewSize.Height <= maxSize.Height && e.NewSize.Width <= maxSize.Width);

                if (minAccepted && maxAccepted)
                {
                    selected = template;
                }
            }

            this.ContentTemplate = selected;
        }
    }
}
