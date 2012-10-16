using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using Ijv.Redstone;
using Ijv.Redstone.Services;

namespace Ijv.Platform.Clients.Data
{
    /// <summary>
    /// Clipboard wrapper that works with internet explorer java script interop.
    /// </summary>
    /// <remarks>
    /// NOTE: The internet explorer browser supports the following data formats:
    /// Text, URL, File, HTML, Image
    /// </remarks>
    [Export(typeof(IClipboard))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class IEClipboard : IClipboard
    {
        /// <summary>
        /// The text format.
        /// </summary>
        private const string TextFormat = "Text";

        /// <summary>
        /// The amount of time in milliseconds that we will allow the cache to live.  Ideally, this value can be quite small, as we are just trying
        /// to aggregate multiple requests with in a dispatcher frame or two.
        /// </summary>
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMilliseconds(250);

        /// <summary>
        /// The last time that the GetText() method accessed the actual IE clipboard for its value.  If it was less than CacheDuration
        /// then we will returned the cached value.  otherwise we will access it agian.
        /// </summary>
        private DateTime cacheLastRefreshed = DateTime.MinValue;

        /// <summary>
        /// The last sampled value of the clipboard.  The purpose of this value is to augment multiple requests make to the clipboard at the same
        /// time by eventhandlers.  For example, when selection changes - everybody accesses the clipboard to see if they need to update their
        /// control state.  We will access the clipboard on the first request - then feed the cached value to everybody else.
        /// </summary>
        private string cachedValue;

        /// <summary>
        /// Initializes a new instance of the IEClipboard class.
        /// </summary>
        [ImportingConstructor]
        public IEClipboard()
        {
            Debug.Assert(!Application.Current.IsRunningOutOfBrowser, "When the application is running out of browser, you should be using the SL clipboard because the browser script interop is not available.");
            Debug.Assert(HtmlPage.IsEnabled, "The html DOM Bridge is not enabled.");
            Debug.Assert(HtmlPage.BrowserInformation.ProductName == "MSIE", "Internet Explorer is required to use the IEClipboard.  Script access errors will occur.");
        }

        /// <summary>
        /// Clears data.
        /// </summary>
        public void Clear()
        {
            ScriptObject clipboard = GetBrowserClipboard();

            bool success = (bool)clipboard.Invoke("clearData", TextFormat);
            if (!success)
            {
                throw new InvalidOperationException("Clipboard.Clear() failed to execute.");
            }

            // no need to update the timestamp, we will resample when it expires.
            this.cachedValue = null;
        }

        /// <summary>
        /// Gets data from the clipboard via script object interop.
        /// </summary>
        /// <returns>The current object on the clipboard.</returns>
        public object GetData()
        {
            if (DateTime.Now.Subtract(this.cacheLastRefreshed) > CacheDuration)
            {
                // enough time has passed between requests, we will re-access the
                // clipboard and resample for a different value.
                ScriptObject clipboard = GetBrowserClipboard();

                object data = clipboard.Invoke("getData", TextFormat);
                this.cachedValue = data as string;
                this.cacheLastRefreshed = DateTime.Now;
            }

            return this.cachedValue;
        }

        /// <summary>
        /// Sets data to the clipboard via script object interop.
        /// </summary>
        /// <param name="data">The object to set to the clipboard.</param>
        public void SetData(object data)
        {
            // preconditions

            Argument.IsNotNull("data", data);

            // implementation

            // TODO: perform serialization work here.
            string text = data.ToString();

            ScriptObject clipboard = GetBrowserClipboard();

            bool success = (bool)clipboard.Invoke("setData", TextFormat, text);
            if (!success)
            {
                throw new InvalidOperationException("Clipboard.Clear() failed to execute.");
            }

            this.cachedValue = text;
            this.cacheLastRefreshed = DateTime.Now;
        }

        /// <summary>
        /// Ensures the browser clipboard script object is created.
        /// </summary>
        /// <returns>The html DOM script object that is the browser's clipboard data.</returns>
        private static ScriptObject GetBrowserClipboard()
        {
            // if the html page is enabled, then we will snag our clipboard.
            if (HtmlPage.IsEnabled)
            {
                ScriptObject scriptObject = (ScriptObject)HtmlPage.Window.GetProperty("clipboardData");

                // if we failed to attain the clipboard, throw an exception.
                if (scriptObject == null)
                {
                    throw new InvalidOperationException("Clipboard.EnsureClipboard() failed to access clipboard.");
                }

                return scriptObject;
            }
            
            return null;
        }
    }
}
