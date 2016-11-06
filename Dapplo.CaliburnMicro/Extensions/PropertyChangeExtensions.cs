using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.Extensions
{
	/// <summary>
	/// Extensions for DependencyObject s
	/// </summary>
	public static class PropertyChangeExtensions
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		/// Create an observable for the INotifyPropertyChanged
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
						return string.IsNullOrEmpty(propertyName) || propertyName == "*" || propertyNamePattern == propertyName || Regex.IsMatch(propertyName, propertyNamePattern);
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
		/// Create an observable for the INotifyPropertyChanged, which returns the EventPattern containing the source
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
				predicate = eventPattern =>
				{
					try
					{
						var propertyName = eventPattern.EventArgs.PropertyName;
						return string.IsNullOrEmpty(propertyName) || propertyName == "*" || propertyNamePattern  == propertyName || Regex.IsMatch(propertyName, propertyNamePattern);
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
		/// Create an observable for the INotifyPropertyChanging
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
						return string.IsNullOrEmpty(propertyName) || propertyName == "*" || propertyNamePattern == propertyName || Regex.IsMatch(propertyName, propertyNamePattern);
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
		/// Create an observable for the INotifyPropertyChanging, which returns the EventPattern containing the source
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
				predicate = eventPattern =>
				{
					try
					{
						var propertyName = eventPattern.EventArgs.PropertyName;
						return string.IsNullOrEmpty(propertyName) || propertyName == "*" || propertyNamePattern == propertyName || Regex.IsMatch(propertyName, propertyNamePattern);
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
