using Application.Demo.MetroAddon.Services;
using Application.Demo.MetroAddon.ViewModels;
using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Configuration;

namespace Application.Demo.MetroAddon
{
    /// <inheritdoc />
    public class MetroAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigureDefaults>().As<IService>().SingleInstance();
            builder.RegisterType<CredentialsViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ThemeConfigViewModel>().As<IConfigScreen>().SingleInstance();

            base.Load(builder);
        }
    }
}
