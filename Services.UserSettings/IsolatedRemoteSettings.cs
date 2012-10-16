using System;
using System.Windows;

namespace Ijv.Redstone.Services.UserSettings
{
    /// <summary />
    public class IsolatedRemoteSettings
    {
        /// <summary />
        public IsolatedRemoteSettings(object key)
        {

        }


        // TODO: Write server implementation.
        // Save to server adn save locally
        // load from disk, and when server completes, load from server.
        // shoudl contain a timestamp.

        /// <summary />
        internal void Save()
        {

        }

        private void ClearRemoteSettings()
        {
            Uri storageUri = new Uri(Application.Current.Host.Source, "/ClientSettingsStorage.svc");

            //            EndpointAddress endpoint = new EndpointAddress(storageUri);
            //            Binding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

            //            ClientSettingsStorageClient client = new ClientSettingsStorageClient(binding, endpoint);

            //            IClientSettingsStorage storage = client.ChannelFactory.CreateChannel();

            AsyncCallback callback = delegate(IAsyncResult result)
            {
                //                    try
                //                    {
                //                        IClientSettingsStorage storage2 = (IClientSettingsStorage)storage;
                //                        storage2.EndClear(result);
                //                    }
                //                    catch (CommunicationException)
                //                    {
                //                        throw;
                // an error occured while clearing the settings
                //                    }
                //                    finally
                //                    {
                //                        this.RaiseSettingsClearedEvent();
                //                    }
            };

            //            storage.BeginClear(callback, storage);
        }
    }
}
