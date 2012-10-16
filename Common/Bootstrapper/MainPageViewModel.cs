using Ijv.Redstone.Design;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using Ijv.Redstone.Services;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace Ijv.Redstone
{
    public class MainPageViewModel : ViewModel
    {
        /// <summary>
        /// Identifies the Content bindable property.
        /// </summary>
        public static readonly BindableProperty ContentProperty = BindableProperty.Register<IViewModel>(
            "Content",
            typeof(MainPageViewModel));

        /// <summary>
        /// The application 
        /// </summary>
        Application application;

        /// <summary>
        /// The entry point for the application.
        /// </summary>
        Type entrypointType;

        /// <summary>
        /// The collection of catalogs that have been loaded into the composition container.
        /// </summary>
        private AggregateCatalog catalogCollection;

        /// <summary>
        /// Contains a list of pending uri's that are being downloaded.
        /// </summary>
        List<DeploymentCatalog> pendingDeployments = new List<DeploymentCatalog>();

        /// <summary>
        /// Initializes a new instance of the MainPageViewModel class. 
        /// </summary>
        public MainPageViewModel(Application app, Type entrypoint)
            : base(new MainPage())
        {
            // preconditions

            Argument.IsNotNull("app", app);
            Argument.IsNotNull("entrypoint", entrypoint);

            // implementation

            this.application = app;
            this.entrypointType = entrypoint;

            this.application.Startup += this.WhenApplicationStartsup;
            this.application.UnhandledException += this.WhenApplicationGetsAnUnhandledException;
            this.application.Exit += this.WhenApplicationExits;
        }

        /// <summary>
        /// Gets or sets the searchbox column width.
        /// </summary>
        public IViewModel Content
        {
            get { return this.GetValue<IViewModel>(ContentProperty); }
            set { this.SetValue<IViewModel>(ContentProperty, value); }
        }

        /// <summary />
        private void WhenApplicationStartsup(object sender, StartupEventArgs e)
        {
            this.InitializeComposition();

            // assign our visual to the screen.
            ((Application)sender).RootVisual = this.View;

            this.Dispatcher.BeginInvoke(this.InitializeCatalogs);
        }

        /// <summary />
        private void WhenApplicationExits(object sender, System.EventArgs e)
        {
            // TODO: what do we need to do when we exit?
            //  1) send shutdown message
        }

        /// <summary />
        private void WhenApplicationGetsAnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // TODO: frost the UI and display an error screen.
        }

        /// <summary />
        private void InitializeComposition()
        {
            this.catalogCollection = new AggregateCatalog();

            // create the container and initialize the container
            CompositionContainer container = new CompositionContainer(this.catalogCollection);
            CompositionHost.Initialize(container);
            ServiceLocator.SetLocatorProvider(container);

            // load the current deployment
            catalogCollection.Catalogs.Add(new DeploymentCatalog());
        }

        /// <summary />
        private void InitializeCatalogs()
        {
            // load remote catalogs
            IDictionary<string, string> args = Application.Current.Host.InitParams;
            if (args != null && args.ContainsKey("xap"))
            {
                string value = args["xap"];

                string[] collection = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string xap in collection)
                {
                    bool absoluteUri = xap.IndexOf("://", StringComparison.OrdinalIgnoreCase) >= 0;
                    Uri uri = new Uri(xap, absoluteUri ? UriKind.Absolute : UriKind.Relative);

                    DeploymentCatalog dc = new DeploymentCatalog(uri);
                    dc.DownloadCompleted += this.DownloadCompleted;
                    this.pendingDeployments.Add(dc);
                    this.catalogCollection.Catalogs.Add(dc);

                    dc.DownloadAsync();
                }
            }
        }

        /// <summary />
        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DeploymentCatalog catalog = (DeploymentCatalog)sender;
            catalog.DownloadCompleted -= this.DownloadCompleted;
            this.pendingDeployments.Remove(catalog);
            
            if (this.pendingDeployments.Count == 0)
            {
                // create an instance of our viewmodel and load it into the content.
                object instance = Activator.CreateInstance(this.entrypointType);
                ((MainPageViewModel)((MainPage)this.application.RootVisual).DataContext).Content = instance as IViewModel;

                // TODO: try catch this and display an error message if we fail to load for debugging purposes
            }
        }

    }
}
