using System.Windows;
using Dapplo.CaliburnMicro.Behaviors.Security;

namespace Dapplo.CaliburnMicro.Menu
{
	/// <summary>
	/// Makes binding possible
	/// </summary>
	public class AuthenticatedVisibilityMenuItem : AuthenticatedMenuItem<Visibility>
	{
		/// <summary>
		/// Set defaults
		/// </summary>
		public AuthenticatedVisibilityMenuItem()
		{
			AuthenticationTargetProperty = AuthenticationTargetProperties.Visibility;
			WhenPermission = Visibility.Visible;
			WhenPermissionMissing = Visibility.Collapsed;
		}
	}
}
