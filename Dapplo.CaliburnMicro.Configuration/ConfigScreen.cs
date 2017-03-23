using Dapplo.CaliburnMicro.Menu;

namespace Dapplo.CaliburnMicro.Configuration
{
	/// <summary>
	/// A screen for the config must implement this
	/// </summary>
	public class ConfigScreen : TreeScreen<IConfigScreen>, IConfigScreen
	{
		/// <inheritdoc />
		public virtual void Initialize(IConfig config)
		{

		}
	}
}
