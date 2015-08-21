// MvxNotifyPropertyChangedEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
{
    public class MvxNotifyPropertyChangedEventSubscription
        : MvxWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        private static readonly EventInfo PropertyChangedEventInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged");

        // This code ensures the PropertyChanged event is not stripped by Xamarin linker
        // see https://github.com/MvvmCross/MvvmCross/pull/453
        public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged)
        {
            iNotifyPropertyChanged.PropertyChanged += (sender, e) => { };
        }

        public MvxNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                         EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, PropertyChangedEventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler(OnSourceEvent);
        }

        protected override void RemoveEventHandler()
        {
            if (!_subscribed)
                return;

            var source = (INotifyPropertyChanged)_sourceReference.Target;
            if (source != null)
            {
                source.PropertyChanged -= (PropertyChangedEventHandler)_ourEventHandler;
                //_sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = false;
            }
        }

        protected override void AddEventHandler()
        {
            if (_subscribed)
                throw new MvxException("Should not call _subscribed twice");

            var source = (INotifyPropertyChanged)_sourceReference.Target;
            if (source != null)
            {
                source.PropertyChanged += (PropertyChangedEventHandler)_ourEventHandler;
                //_sourceEventInfo.GetAddMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }
}