using Dapplo.CaliburnMicro.Behaviors.Security;

namespace Dapplo.CaliburnMicro.Menu
{
	/// <summary>
	/// AuthenticatedMenuItem for the IsEnabled
	/// </summary>
	public class AuthenticatedIsEnabledMenuItem : AuthenticatedMenuItem<bool>
	{
		/// <summary>
		/// Set defaults
		/// </summary>
		public AuthenticatedIsEnabledMenuItem()
		{
			AuthenticationTargetProperty = AuthenticationTargetProperties.IsEnabled;
			WhenPermission = true;
			WhenPermissionMissing = false;
		}
	}
}
