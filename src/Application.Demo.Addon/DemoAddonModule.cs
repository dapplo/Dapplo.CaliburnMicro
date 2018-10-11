using Application.Demo.Addon.Languages;
using Application.Demo.Addon.Languages.Impl;
using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Configuration;

namespace Application.Demo.Addon
{
    /// <summary>
    /// Setup the demo addon
    /// </summary>
    public class DemoAddonModule : AddonModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddonTranslationsImpl>()
                .As<IAddonTranslations>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IConfigScreen>()
                .As<IConfigScreen>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
