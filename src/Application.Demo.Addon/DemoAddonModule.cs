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
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IConfigScreen>()
                .As<IConfigScreen>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
