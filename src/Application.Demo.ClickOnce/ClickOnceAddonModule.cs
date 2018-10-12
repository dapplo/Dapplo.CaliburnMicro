using Application.Demo.ClickOnce.Config;
using Application.Demo.ClickOnce.Config.Impl;
using Application.Demo.ClickOnce.ViewModels;
using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.ClickOnce;
using Dapplo.Config.Ini;

namespace Application.Demo.ClickOnce
{
    public class ClickOnceAddonModule : AddonModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClickOnceDemoConfigurationImpl>()
                .As<IClickOnceDemoConfiguration>()
                .As<IIniSection>()
                .SingleInstance();

            builder.RegisterType<HandleClickOnceRestarts>()
                .As<IHandleClickOnceRestarts>()
                .SingleInstance();

            builder.RegisterType<AboutViewModel>()
                .As<IShell>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
