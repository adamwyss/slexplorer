using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ijv.Redstone.Design
{
    /// <summary>
    public abstract class ViewModel : BindableObject, IViewModel
    {
        #region Static Bindable Property Definitions

        /// <summary>
        /// Identifies the View bindable property.
        /// </summary>
        public static readonly BindableProperty ViewProperty = BindableProperty.Register<Control>(
            "View",
            typeof(ViewModel));

        #endregion

        /// <summary>
        /// Creates an instance of the <see cref="ViewModel" /> class.
        /// </summary>
        /// <param name="view">The view that represents this view model.</param>
        protected ViewModel(Control view)
            : this()
        {
            // preconditions

            Argument.IsNotNull("view", view);

            // implementation

            this.View = view;
        }

        /// <summary>
        /// Creates an instance of the <see cref="ViewModel" /> class.
        /// </summary>
        protected ViewModel()
        {
            // delay the execution of this method until the current callstack
            // completes.  This allows all derived types to complete their
            // initialization in the constructor.
            this.Dispatcher.BeginInvoke(this.OnViewModelLoaded);
        }

        /// <summary>
        /// Gets the view that is attached to this viewmodel.
        /// </summary>
        public Control View
        {
            get
            {
                this.EnsureDataContext();
                return this.GetValue<Control>(ViewProperty);
            }

            internal protected set
            {
                this.SetValue<Control>(ViewProperty, value);
            }
        }

        /// <summary>
        /// Gets the <see ref="Dispatcher" /> this view model is associated with.
        /// </summary>
        protected Dispatcher Dispatcher
        {
            get
            {
                return Deployment.Current.Dispatcher;
            }
        }
       
        /// <summary>
        /// Occurs after the constructor call stack has completed, allowing all derived classes to complete their 
        /// initialization before assigning the view's data context.
        /// </summary>
        private void OnViewModelLoaded()
        {
            this.EnsureDataContext();
        }

        bool busy = false;

        /// <summary>
        private void EnsureDataContext()
        {
            if (busy)
            {
                return;
            }

            try
            {
                this.busy = true;

                if (this.View != null)
                {
                    if (this.View.DataContext == null)
                    {
                        this.View.DataContext = this;
                    }
                }
            }
            finally
            {
                this.busy = false;
            }
        }
    }
}
