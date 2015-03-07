using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfBehaviours.Infrastructure.ExtensionMethods;

namespace WpfBehaviours.Infrastructure.ViewModel
{
    public class INPCBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Warns the developer if this object does not have a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {

            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);

                Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, args);
            }
        }

        //Used by code that may want to fire additional properties. For example
        //when there is only a getter available, but the UI still needs to be notified


        /// <summary>
        /// Used by code that may want to fire additional properties. For example
        /// when there is only a getter available, but the UI still needs to be n
        /// </summary>
        /// <param name="propertyName">Expression representing the property to use</param>
        public void RaisePropertyChanged<TValue>(Expression<Func<TValue>> propertySelector)
        {
            if (PropertyChanged != null)
            {
                NotifyPropertyChanged(new PropertyChangedEventArgs(propertySelector.GetPropertyName()));
            }
        }

        public TRet RaiseAndSetIfChanged<TRet, TValue>(ref TRet backingField, TRet newValue, Expression<Func<TValue>> propertySelector)
        {
            if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                return newValue;
            }

            backingField = newValue;
            RaisePropertyChanged(propertySelector);
            return newValue;
        }

        public TRet RaiseAndSet<TRet, TValue>(ref TRet backingField, TRet newValue, Expression<Func<TValue>> propertySelector)
        {
            backingField = newValue;
            RaisePropertyChanged(propertySelector);
            return newValue;
        }


    }
}
