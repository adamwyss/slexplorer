using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Runtime.Serialization;

namespace Ijv.Redstone.Services.UserSettings
{
    /// <summary />
    public class IsolatedLocalSettings : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IDictionary, ICollection, IEnumerable
    {
        /// <summary />
        private string storagePath;

        /// <summary />
        private Dictionary<string, object> settings;

        /// <summary />
        public IsolatedLocalSettings(string settingsName)
        {
            Argument.IsNotNullOrEmpty("settingsName", settingsName);

            // implementation

            this.storagePath = settingsName;
            this.Reload();
        }

        /// <summary />
        public int Count
        {
            get
            {
                return this.settings.Count;
            }
        }

        /// <summary />
        public object this[string key]
        {
            get
            {
                // preconditions

                Argument.IsNotNull("key", key);

                // implementation

                return this.settings[key];
            }

            set
            {
                // preconditions

                Argument.IsNotNull("key", key);

                // implementation

                this.settings[key] = value;
            }
        }

        /// <summary />
        public ICollection Keys
        {
            get
            {
                return this.settings.Keys;
            }
        }

        /// <summary />
        public ICollection Values
        {
            get
            {
                return this.settings.Values;
            }
        }
        
        /// <summary />
        public void Add(string key, object value)
        {
 	        // preconditions

            Argument.IsNotNull("key", key);

            // implementation

            this.settings.Add(key, value);
        }

        /// <summary />
        public void Clear()
        {
            this.settings.Clear();
        }

        /// <summary />
        public bool Contains(string key)
        {
            // preconditions

            Argument.IsNotNull("key", key);

            // implementation

            return this.settings.ContainsKey(key);
        }

        /// <summary />
        public bool Remove(string key)
        {
            // preconditions

            Argument.IsNotNull("key", key);

            // implementation

            return this.settings.Remove(key);
        }

        /// <summary />
        public bool TryGetValue<T>(string key, out T value)
        {
            value = default(T);

            if (key != null)
            {
                object obj2;

                if (this.settings.TryGetValue(key, out obj2))
                {
                    value = (T)obj2;
                    return true;
                }
            }

            return false;
        }

        /// <summary />
        internal void Save()
        {
            if (this.settings.Count > 0)
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(this.storagePath, FileMode.Create, storage))
                    {
                        using (MemoryStream stream2 = new MemoryStream())
                        {
                            Dictionary<Type, bool> dictionary = new Dictionary<Type, bool>();
                            StringBuilder builder = new StringBuilder();
                            foreach (object obj2 in this.settings.Values)
                            {
                                if (obj2 != null)
                                {
                                    Type type = obj2.GetType();
                                    if (!type.IsPrimitive && (type != typeof(string)))
                                    {
                                        dictionary[type] = true;
                                        if (builder.Length > 0)
                                        {
                                            builder.Append("\0");
                                        }
                                        builder.Append(type.AssemblyQualifiedName);
                                    }
                                }
                            }
                            builder.Append(Environment.NewLine);
                            byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());
                            stream2.Write(bytes, 0, bytes.Length);
                            new DataContractSerializer(typeof(Dictionary<string, object>), dictionary.Keys).WriteObject(stream2, this.settings);
                            stream.SetLength(0L);
                            byte[] buffer = stream2.ToArray();
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }

        /// <summary />
        private void Reload()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(this.storagePath))
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(this.storagePath, FileMode.Open, storage))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            if (stream.Length > 0L)
                            {
                                try
                                {
                                    List<Type> knownTypes = new List<Type>();
                                    string str = reader.ReadLine();
                                    foreach (string str2 in str.Split(new char[1]))
                                    {
                                        Type item = Type.GetType(str2, false);
                                        if (item != null)
                                        {
                                            knownTypes.Add(item);
                                        }
                                    }
                                    stream.Position = str.Length + Environment.NewLine.Length;
                                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), knownTypes);
                                    this.settings = (Dictionary<string, object>)serializer.ReadObject(stream);
                                }
                                catch (Exception)
                                {
                                    // eat any exception - we will assume the file is corrupt and reset the settings
                                    // by providing an empty dictionary.
                                    try
                                    {
                                        storage.DeleteFile(this.storagePath);
                                    }
                                    catch (IsolatedStorageException)
                                    {
                                        // the file must be locked,
                                    }

                                }
                            }
                        }
                    }
                }
            }

            if (this.settings == null)
            {
                // initialize the settings dictionary.
                this.settings = new Dictionary<string, object>();
            }
        }

















        #region IDictionary<string, object> Explicit Implementation

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ICollection<KeyValuePair<string, object>> Explicit Implementation

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerator<KeyValuePair<string, object>> Explicit Implementation

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDictionary Explicit Implementation

        void IDictionary.Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        void IDictionary.Clear()
        {
            throw new NotImplementedException();
        }

        bool IDictionary.Contains(object key)
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        bool IDictionary.IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionary.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        ICollection IDictionary.Keys
        {
            get { throw new NotImplementedException(); }
        }

        void IDictionary.Remove(object key)
        {
            throw new NotImplementedException();
        }

        ICollection IDictionary.Values
        {
            get { throw new NotImplementedException(); }
        }

        object IDictionary.this[object key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Explicit Implementation

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        bool ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerator Explicit Implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
