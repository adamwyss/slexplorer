using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace Ijv.Redstone.Design
{
    /// <summary>
    /// Represents a property that is designed to maintain a two-way binding.
    /// </summary>
    /// <remarks>Similar to the Dependency Property.</remarks>
    [DebuggerDisplay("{Name}")]
    public class BindableProperty
    {
        #region Instance Members

        /// <summary>
        /// Creates an instance of the BindableProperty class.
        /// </summary>
        private BindableProperty(string name, object defaultValue, Type ownerType, BindablePropertyChangedCallback changedCallback)
        {
            this.Name = name;
            this.DefaultValue = defaultValue;
            this.OwnerType = ownerType;
            this.ChangedCallback = changedCallback;
        }

        /// <summary>
        /// Gets the name of the bindable property
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the default value of the bindable property.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets the type of the owner of this property.
        /// </summary>
        internal Type OwnerType { get; private set; }

        /// <summary>
        /// Gets the delegate that is executed when this property changes.
        /// </summary>
        internal BindablePropertyChangedCallback ChangedCallback;
 
        #endregion

        #region Static Members

        /// <summary>
        /// A dictionary of all registered bindable properties.
        /// </summary>
        private static Dictionary<int, BindableProperty> registeredProperties = new Dictionary<int, BindableProperty>();

        /// <summary>
        /// Registers a bindable property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="name">The name of the bindable property to register. The name must be unique within the registration namespace of the owner type.</param>
        /// <param name="ownerType">The owner type that is registering the bindable property.</param>
        /// <returns>A bindable property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the bindable property later, for operations such as setting its value programmatically.</returns>
        public static BindableProperty Register<T>(string name, Type ownerType)
        {
            return Register<T>(name, ownerType, default(T), null);
        }

        /// <summary>
        /// Registers a bindable property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="name">The name of the bindable property to register. The name must be unique within the registration namespace of the owner type.</param>
        /// <param name="ownerType">The owner type that is registering the bindable property.</param>
        /// <param name="changedCallback">The delegate that is invoked when this bindable property has changed or updated.</param>
        /// <returns>A bindable property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the bindable property later, for operations such as setting its value programmatically.</returns>
        public static BindableProperty Register<T>(string name, Type ownerType, BindablePropertyChangedCallback changedCallback)
        {
            return Register<T>(name, ownerType, default(T), changedCallback);
        }

        /// <summary>
        /// Registers a bindable property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="name">The name of the bindable property to register. The name must be unique within the registration namespace of the owner type.</param>
        /// <param name="ownerType">The owner type that is registering the bindable property.</param>
        /// <param name="defaultValue">The default value to specify for a dependency property, usually provided as a value of some specific type.</param>
        /// <returns>A bindable property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the bindable property later, for operations such as setting its value programmatically.</returns>
        public static BindableProperty Register<T>(string name, Type ownerType, T defaultValue)
        {
            return Register<T>(name, ownerType, defaultValue, null);
        }

        /// <summary>
        /// Registers a bindable property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="name">The name of the bindable property to register. The name must be unique within the registration namespace of the owner type.</param>
        /// <param name="ownerType">The owner type that is registering the bindable property.</param>
        /// <param name="defaultValue">The default value to specify for a dependency property, usually provided as a value of some specific type.</param>
        /// <param name="changedCallback">The delegate that is invoked when this bindable property has changed or updated.</param>
        /// <returns>A bindable property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the bindable property later, for operations such as setting its value programmatically.</returns>
        public static BindableProperty Register<T>(string name, Type ownerType, T defaultValue, BindablePropertyChangedCallback changedCallback)
        {
            // preconditions

            Argument.IsNotNullOrEmpty("name", name);
            Argument.IsNotNull("ownerType", ownerType);

            // require the owner type maintains the property reference as a static readonly field
            FieldInfo field = ownerType.GetField(name + "Property", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            if (field == null)
            {
                throw new ArgumentException();
            }

            // require that the owner type declare the property as a non-static public property
            PropertyInfo property = ownerType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (property == null)
            {
                throw new ArgumentException();
            }

            // require the property on the object match the type.
            if (property.PropertyType != typeof(T))
            {
                throw new ArgumentException();
            }

            // implementation

            BindableProperty prop = new BindableProperty(name, defaultValue, ownerType, changedCallback);

            lock (((ICollection)registeredProperties).SyncRoot)
            {
                int hash = prop.GetHashCode();
                if (registeredProperties.ContainsKey(hash))
                {
                    throw new InvalidOperationException();
                }
                registeredProperties.Add(hash, prop);
            }

            return prop;
        }

        #endregion
    }
}
