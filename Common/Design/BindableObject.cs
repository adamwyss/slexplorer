using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace Ijv.Redstone.Design
{
    /// <summary>
    /// Represents an object that participates in the bindable property system.
    /// </summary>
    public abstract class BindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Contains the effective values of all dependency properties on this instance.
        /// </summary>
        private Dictionary<int, object> propertyValues = new Dictionary<int, object>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the local value of a bindable property, specified by its dependency property identifier. 
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">The identifier of the dependency property to set.</param>
        /// <param name="value">The new local value.</param>
        public void SetValue<T>(BindableProperty property, T value)
        {
            // preconditions

            Argument.IsNotNull("property", property);
            this.VerifyOwner("property", property);

            // implementation

            int hash = property.GetHashCode();
            object oldValue;

            bool found = propertyValues.TryGetValue(hash, out oldValue);

            // INVESTIGATE : what is the desired behavior around value being null or the default value?
            //  1) accept all values and only remove entry on clearvalue(). (current implementation)
            //  2) remove the value entry when new value is null or default value.
            //  3) clear value entry when new value is null, but accept default value or visa-versa.
            //  NOTE: options 2 & 3 create wierdness around get/set senerio's involving null && default value.

            if (!found)
            {
                propertyValues.Add(hash, value);
                this.RaisePropertyChangedEvents(property, oldValue, value);
            }
            else if (!object.Equals(value, oldValue))
            {
                propertyValues[hash] = value;
                this.RaisePropertyChangedEvents(property, oldValue, value);
            }
        }

        /// <summary>
        /// Returns the current effective value of a bindable property on this instance. 
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">The identifier of the dependency property to get.</param>
        /// <returns>Returns the current effective value.</returns>
        public T GetValue<T>(BindableProperty property)
        {
            // preconditions

            Argument.IsNotNull("property", property);
            this.VerifyOwner("property", property);

            // implementation

            int hash = property.GetHashCode();
            object value;

            bool found = this.propertyValues.TryGetValue(hash, out value);
            if (found)
            {
                return (T)value;
            }

            return (T)property.DefaultValue;
        }

        /// <summary>
        /// Clears the local value of a bindable property, specified by its dependency property identifier. 
        /// </summary>
        /// <param name="property">The identifier of the dependency property to clear.</param>
        public void ClearValue(BindableProperty property)
        {
            // preconditions

            Argument.IsNotNull("property", property);
            this.VerifyOwner("property", property);

            // implementation

            int hash = property.GetHashCode();
            object oldValue;

            bool found = propertyValues.TryGetValue(hash, out oldValue);

            if (found)
            {
                this.propertyValues.Remove(hash);
                this.RaisePropertyChangedEvents(property, oldValue, property.DefaultValue);
            }
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="property">The property who's value changed.</param>
        private void RaisePropertyChangedEvents(BindableProperty property, object oldValue, object newValue)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property.Name));
            }

            BindablePropertyChangedCallback callback = property.ChangedCallback;
            if (callback != null)
            {
                callback(this, property, oldValue, newValue);
            }
        }

        /// <summary>
        /// Verifies the bindable property owner is this document.
        /// </summary>
        /// <param name="name">The name of the property parameter.</param>
        /// <param name="property">The bindable property.</param>
        private void VerifyOwner(string name, BindableProperty property)
        {
            Type t = this.GetType();
            
            // decend and check available base types until we find one that matches
            Type decender = t;
            do
            {
                if (decender == property.OwnerType)
                {
                    return;
                }

                decender = decender.BaseType;
            }
            while (decender != null);

            // if no matching inherited types could be found, throw an exception.
            throw new InvalidOperationException("Bindable properties can only be accessed by the class that delcared them.");
        }
    }
}
