using System;
using System.Diagnostics;
using System.Collections;

namespace Ijv.Redstone
{
    /// <summary>
    /// Provides simple argument checking.
    /// </summary>
    [DebuggerNonUserCode]
    internal static class Argument
    {
        /// <summary>
        /// Throws the appropiate exception if the parameter violates the check.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public static void IsNotNullOrEmpty(string name, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            else if (value.Length == 0)
            {
                throw new ArgumentException(@"The argument string is zero length.", name);
            }
        }

        /// <summary>
        /// Throws the appropiate exception if the parameter violates the check.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public static void IsNotNull(string name, object parameter)
        {
            // preconditions

            IsNotNullOrEmpty("name", name);

            // implementation

            if (parameter == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws the appropiate exception if the parameter violates the check.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public static void AreEqual(string name, object parameter, object value)
        {
            // preconditions

            IsNotNullOrEmpty("name", name);

            // implementation

            if ((parameter == null && value != null) || parameter != null && !parameter.Equals(value))
            {
                throw new ArgumentException("The argument is not equal to the expected value.", name);
            }
        }

        /// <summary>
        /// Throws the appropiate exception if the parameter violates the check.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="parameter">The value of the parameter.</param>
        /// <param name="implementsType">The type that parameter must implement or inherit from.</param>
        public static void Implements(string name, object parameter, Type implementsType)
        {
            // preconditions

            IsNotNull(name, parameter);
            IsNotNull("implementsType", implementsType);

            // implementation

            Type t = parameter.GetType();

            // check each inherited interface and see if it is of the expected type
            Type[] interfaces = t.GetInterfaces();
            foreach (Type @interface in interfaces)
            {
                if (@interface == implementsType)
                {
                    return;
                }
            }
            
            // decend and check available base types until we find one that matches
            Type decender = t;
            do
            {
                if (decender == implementsType)
                {
                    return;
                }

                decender = decender.BaseType;
            }
            while (decender != null);

            // if no matching inherited types could be found, throw an exception.
            throw new ArgumentException("The argument does not inherit from the expected type. ", name);
        }

        /// <summary>
        /// Throws the appropiate exception if the parameter violates the check.
        /// </summary>
        /// <typeparam name="T">The type that parameter must implement or inherit from.</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="parameter">The value of the parameter.</param>
        public static void Implements<T>(string name, object parameter)
        {
            // preconditions

            IsNotNull(name, parameter);

            // implementation

            bool implementsType = parameter is T;
            if (!implementsType)
            {
                throw new ArgumentException("The argument does not inherit from the expected type. ", name);
            }
        }
    }
}
