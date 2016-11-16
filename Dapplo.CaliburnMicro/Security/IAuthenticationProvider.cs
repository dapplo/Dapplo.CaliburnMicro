using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapplo.CaliburnMicro.Security
{
	/// <summary>
	/// Interface which all authentication providers must implement
	/// </summary>
	public interface IAuthenticationProvider
	{
		/// <summary>
		/// Returns if the current user has a certain permission
		/// </summary>
		/// <param name="permission">string with the name of the permission</param>
		/// <returns>true if the current user has the specified permission</returns>
		bool HasPermission(string permission);
	}
}
