using System.Windows;

namespace Dapplo.CaliburnMicro.Security
{
	/// <summary>
	/// Helper extensions to set the default values for INeedAuthentication
	/// </summary>
	public static class NeedAuthenticationExtensions
	{
		/// <summary>
		/// Configure the IChangeableNeedAuthentication to be visible when permission is available, and hidden when not
		/// </summary>
		/// <param name="visibilityAuthentication">INeedAuthentication of type Visibility</param>
		/// <param name="hidden">Visibility for hidden</param>
		public static void VisibleOnPermission(this IChangeableNeedAuthentication<Visibility> visibilityAuthentication, Visibility hidden = Visibility.Collapsed)
		{
			visibilityAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.Visibility;
			visibilityAuthentication.WhenPermission = Visibility.Visible;
			visibilityAuthentication.WhenPermissionMissing = hidden;
		}

		/// <summary>
		/// Configure the IChangeableNeedAuthentication to be visible when permission is available, and hidden when not
		/// </summary>
		/// <param name="visibilityAuthentication">INeedAuthentication of type Visibility</param>
		/// <param name="hidden">Visibility for hidden</param>
		public static void VisibleOnPermissionMissing(this IChangeableNeedAuthentication<Visibility> visibilityAuthentication, Visibility hidden = Visibility.Collapsed)
		{
			visibilityAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.Visibility;
			visibilityAuthentication.WhenPermission = hidden;
			visibilityAuthentication.WhenPermissionMissing = Visibility.Visible;
		}

		/// <summary>
		/// Configure the IChangeableNeedAuthentication to be enabled when permission is available, and disabled when not
		/// </summary>
		/// <param name="enabledAuthentication">INeedAuthentication of type bool</param>
		public static void EnabledOnPermission(this IChangeableNeedAuthentication<bool> enabledAuthentication)
		{
			enabledAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.IsEnabled;
			enabledAuthentication.WhenPermission = true;
			enabledAuthentication.WhenPermissionMissing = false;
		}

		/// <summary>
		/// Configure the IChangeableNeedAuthentication to be visible when permission is available, and hidden when not
		/// </summary>
		/// <param name="enabledAuthentication">INeedAuthentication of type bool</param>
		public static void EnabledOnPermissionMissing(this IChangeableNeedAuthentication<bool> enabledAuthentication)
		{
			enabledAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.IsEnabled;
			enabledAuthentication.WhenPermission = false;
			enabledAuthentication.WhenPermissionMissing = true;
		}
	}
}
