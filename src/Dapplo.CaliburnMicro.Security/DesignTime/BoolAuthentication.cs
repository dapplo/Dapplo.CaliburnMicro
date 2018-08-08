using System.Collections.Generic;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Security.DesignTime
{
#if DEBUG
    /// <summary>
    /// A designtime implementation of INeedAuthentication for the bool (IsEnabled)
    /// </summary>
    public class BoolAuthentication : INeedAuthentication<bool>
    {
        /// <inheritdoc />
        public AuthenticationTargetProperties AuthenticationTargetProperty { get; set; }
        /// <inheritdoc />
        public PermissionOperations PermissionOperation { get; set; }
        /// <inheritdoc />
        public IEnumerable<string> Permissions { get; set; }
        /// <inheritdoc />
        public bool WhenPermission { get; set; }
        /// <inheritdoc />
        public bool WhenPermissionMissing { get; set; }
    }
#endif
}
