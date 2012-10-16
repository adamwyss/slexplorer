using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a control with a single piece of content.
    /// </summary>
    public class DataTemplatePresenter : ContentControl
    {
        /// <summary>
        /// Creates a new instance of the DataTemplatePresenter control.
        /// </summary>
        public DataTemplatePresenter()
        {
            this.DefaultStyleKey = typeof(DataTemplatePresenter);
        }

        /// <summary>
        /// Gets or sets a template selector that enables an application writer to provide custom template-selection logic.
        /// </summary>
        public DataTemplateSelector ContentTemplateSelector { get; set; }

        /// <summary>
        /// Called whenever application code or internal processes call ApplyTemplate and updates the content template property.
        /// </summary>
        public override void OnApplyTemplate()
        {
            bool success = this.UpdateDataTemplate();
            
            if (!success)
            {
                base.OnApplyTemplate();
            }
        }

        /// <summary>
        /// Called when the content property changes and updates the content template property.
        /// </summary>
        /// <param name="oldContent">The old value of the content property.</param>
        /// <param name="newContent">The new value of the content property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            this.UpdateDataTemplate();
        }

        /// <summary>
        /// Updates the content template of this control.
        /// </summary>
        /// <returns>True if the template was successfully updated, otherwise false.</returns>
        private bool UpdateDataTemplate()
        {
            DataTemplate template = null;

            if (this.ContentTemplateSelector != null)
            {
                template = this.ContentTemplateSelector.SelectTemplate(this.Content, this);
            }
            else
            {
                // TODO: This should be assigned in the style -> so the default data template selector 
                // can be easily configured and updated.
                template = new TypeKeyedResourceDataTemplateSelector().SelectTemplate(this.Content, this);
            }

            if (template != null)
            {
                this.ContentTemplate = template;
                return true;
            }

            return false;
        }
    }
}
