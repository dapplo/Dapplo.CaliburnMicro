using System;
using System.Windows.Data;
using Dapplo.CaliburnMicro.Behaviors.Security;

namespace Dapplo.CaliburnMicro.Converters
{
	/// <summary>
	/// Returns AuthenticationTargetProperties from INeedAuthentication, or AuthenticationTargetProperties.None
	/// </summary>
	public class AuthenticationTargetPropertyConverter : IValueConverter
	{
		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null)
			{
				var needAuthentication = value as INeedAuthentication;
				return needAuthentication?.AuthenticationTargetProperty ?? AuthenticationTargetProperties.None;
			}
			return AuthenticationTargetProperties.None;
		}

		/// <inheritdoc />
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
