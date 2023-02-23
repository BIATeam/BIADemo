// <copyright file="Unsubscriber.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Common.Observer
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class allow to passed a reference to the observers collection and a reference to the observer that is added to the collection.
    /// These references are assigned to local variables. When the object's Dispose method is called, it checks whether the observer still
    /// exists in the observers collection, and, if it does, removes the observer.
    /// </summary>
    /// <typeparam name="T">The object that provides notification information.</typeparam>
    public sealed class Unsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> observers;
        private readonly IObserver<T> observer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unsubscriber{T}"/> class.
        /// </summary>
        /// <param name="observers">The whole list of observer.</param>
        /// <param name="observer">The current observer.</param>
        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }

        /// <summary>
        /// Removes the observer from observer collection.
        /// </summary>
        public void Dispose()
        {
            if (this.observers.Contains(this.observer))
            {
                this.observers.Remove(this.observer);
            }
        }
    }
}
