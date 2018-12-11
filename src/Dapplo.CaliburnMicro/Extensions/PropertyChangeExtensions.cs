//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Dapplo.Log;

#endregion

namespace Dapplo.CaliburnMicro.Extensions
{
    /// <summary>
    ///     Extensions for DependencyObject s
    /// </summary>
    public static class PropertyChangeExtensions
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        ///     Create an observable for the INotifyPropertyChanged
        /// </summary>
        /// <param name="source">INotifyPropertyChanged</param>
        /// <param name="propertyNamePattern">Optional property name / pattern</param>
        /// <typeparam name="T">INotifyPropertyChanged</typeparam>
        /// <returns>IObservable with PropertyChangedEventArgs</returns>
        public static IObservable<PropertyChangedEventArgs> OnPropertyChanged<T>(this T source, string propertyNamePattern = null)
            where T : INotifyPropertyChanged
        {
            var observable = Observable.Create<PropertyChangedEventArgs>(observer =>
            {
                PropertyChangedEventHandler handler = (s, e) => observer.OnNext(e);
                source.PropertyChanged += handler;
                return Disposable.Create(() => source.PropertyChanged -= handler);
            });

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Create predicate
            Func<PropertyChangedEventArgs, bool> predicate;
            if (!string.IsNullOrEmpty(propertyNamePattern) && propertyNamePattern != "*")
            {
                predicate = propertyChangedEventArgs =>
                {
                    try
                    {
                        var propertyName = propertyChangedEventArgs.PropertyName;
                        if (string.IsNullOrEmpty(propertyName))
                        {
                            return true;
                        }
                        if (propertyNamePattern.Contains("*") || propertyNamePattern.Contains("."))
                        {
                            if (Regex.IsMatch(propertyName, propertyNamePattern))
                            {
                                return true;
                            }
                        }

                        return propertyName == "*" || propertyNamePattern == propertyName;
                    }
                    catch (Exception ex)
                    {
                        Log.Error().WriteLine(ex, "Error in predicate for OnPropertyChangedPattern");
                    }
                    return false;
                };
            }
            else
            {
                predicate = args => true;
            }

            return observable.Where(predicate);
        }

        /// <summary>
        ///     Create an observable for the INotifyPropertyChanged, which returns the EventPattern containing the source
        /// </summary>
        /// <param name="source">INotifyPropertyChanged</param>
        /// <param name="propertyNamePattern">Optional property name / pattern</param>
        /// <typeparam name="T">INotifyPropertyChanged</typeparam>
        /// <returns>IObservable with EventPattern of PropertyChangedEventArgs</returns>
        public static IObservable<EventPattern<PropertyChangedEventArgs>> OnPropertyChangedPattern<T>(this T source, string propertyNamePattern = null)
            where T : INotifyPropertyChanged
        {
            var observable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => handler.Invoke,
                h => source.PropertyChanged += h,
                h => source.PropertyChanged -= h);

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Create predicate
            Func<EventPattern<PropertyChangedEventArgs>, bool> predicate;
            if (!string.IsNullOrEmpty(propertyNamePattern) && propertyNamePattern != "*")
            {
                predicate = propertyChangedEventPattern =>
                {
                    try
                    {
                        var propertyName = propertyChangedEventPattern.EventArgs.PropertyName;
                        if (string.IsNullOrEmpty(propertyName))
                        {
                            return true;
                        }
                        if (propertyNamePattern.Contains("*") || propertyNamePattern.Contains("."))
                        {
                            if (Regex.IsMatch(propertyName, propertyNamePattern))
                            {
                                return true;
                            }
                        }

                        return propertyName == "*" || propertyNamePattern == propertyName;
                    }
                    catch (Exception ex)
                    {
                        Log.Error().WriteLine(ex, "Error in predicate for OnPropertyChangedPattern");
                    }
                    return false;
                };
            }
            else
            {
                predicate = args => true;
            }

            return observable.Where(predicate);
        }

        /// <summary>
        ///     Create an observable for the INotifyPropertyChanging
        /// </summary>
        /// <param name="source">INotifyPropertyChanging</param>
        /// <param name="propertyNamePattern">Optional property name / pattern</param>
        /// <typeparam name="T">INotifyPropertyChanging</typeparam>
        /// <returns>IObservable with PropertyChangingEventArgs</returns>
        public static IObservable<PropertyChangingEventArgs> OnPropertyChanging<T>(this T source, string propertyNamePattern = null)
            where T : INotifyPropertyChanging
        {
            var observable = Observable.Create<PropertyChangingEventArgs>(observer =>
            {
                PropertyChangingEventHandler handler = (s, e) => observer.OnNext(e);
                source.PropertyChanging += handler;
                return Disposable.Create(() => source.PropertyChanging -= handler);
            });

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Create predicate
            Func<PropertyChangingEventArgs, bool> predicate;
            if (!string.IsNullOrEmpty(propertyNamePattern) && propertyNamePattern != "*")
            {
                predicate = propertyChangedEventArgs =>
                {
                    try
                    {
                        var propertyName = propertyChangedEventArgs.PropertyName;
                        if (string.IsNullOrEmpty(propertyName))
                        {
                            return true;
                        }
                        if (propertyNamePattern.Contains("*") || propertyNamePattern.Contains("."))
                        {
                            if (Regex.IsMatch(propertyName, propertyNamePattern))
                            {
                                return true;
                            }
                        }

                        return propertyName == "*" || propertyNamePattern == propertyName;
                    }
                    catch (Exception ex)
                    {
                        Log.Error().WriteLine(ex, "Error in predicate for OnPropertyChanging");
                    }
                    return false;
                };
            }
            else
            {
                predicate = args => true;
            }

            return observable.Where(predicate);
        }

        /// <summary>
        ///     Create an observable for the INotifyPropertyChanging, which returns the EventPattern containing the source
        /// </summary>
        /// <param name="source">INotifyPropertyChanging</param>
        /// <param name="propertyNamePattern">Optional property name / pattern</param>
        /// <typeparam name="T">INotifyPropertyChanging</typeparam>
        /// <returns>IObservable with EventPattern of PropertyChangingEventArgs</returns>
        public static IObservable<EventPattern<PropertyChangingEventArgs>> OnPropertyChangingPattern<T>(this T source, string propertyNamePattern = null)
            where T : INotifyPropertyChanging
        {
            var observable = Observable.FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(
                handler => handler.Invoke,
                h => source.PropertyChanging += h,
                h => source.PropertyChanging -= h);

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Create predicate
            Func<EventPattern<PropertyChangingEventArgs>, bool> predicate;
            if (!string.IsNullOrEmpty(propertyNamePattern) && propertyNamePattern != "*")
            {
                predicate = propertyChangingEventPattern =>
                {
                    try
                    {
                        var propertyName = propertyChangingEventPattern.EventArgs.PropertyName;
                        if (string.IsNullOrEmpty(propertyName))
                        {
                            return true;
                        }
                        if (propertyNamePattern.Contains("*") || propertyNamePattern.Contains("."))
                        {
                            if (Regex.IsMatch(propertyName, propertyNamePattern))
                            {
                                return true;
                            }
                        }

                        return propertyName == "*" || propertyNamePattern == propertyName;
                    }
                    catch (Exception ex)
                    {
                        Log.Error().WriteLine(ex, "Error in predicate for OnPropertyChangingPattern");
                    }
                    return false;
                };
            }
            else
            {
                predicate = args => true;
            }

            return observable.Where(predicate);
        }
    }
}