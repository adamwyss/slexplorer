using System.Windows.Media;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a data structure that contains piece of text and an image.
    /// </summary>
    public class RibbonLabel
    {
        /// <summary>
        /// Initializes a new instance of the TextImageThingie class.
        /// </summary>
        public RibbonLabel()
        {
        }

        /// <summary>
        /// Gets or sets the display text associated with this label.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the image assocated with this label.
        /// </summary>
        public ImageSource Image { get; set; }
    }
}
