using System.Collections.Generic;
using System.Windows;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Security.DesignTime
{
#if DEBUG
    /// <summary>
    /// A designtime implementation of INeedAuthentication for the visibility
    /// </summary>
    public class VisibilityAuthentication : INeedAuthentication<Visibility>
    {
        /// <inheritdoc />
        public AuthenticationTargetProperties AuthenticationTargetProperty { get; set; }
        /// <inheritdoc />
        public PermissionOperations PermissionOperation { get; set; }
        /// <inheritdoc />
        public IEnumerable<string> Permissions { get; set; }
        /// <inheritdoc />
        public Visibility WhenPermission { get; set; }
        /// <inheritdoc />
        public Visibility WhenPermissionMissing { get; set; }
    }
#endif
}
