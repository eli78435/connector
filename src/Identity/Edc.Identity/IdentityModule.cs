using Autofac;
using Edc.Identity.Engines;
using Edc.Identity.Managers;

namespace Edc.Identity;

public class IdentityModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UsersEngine>().As<IUsersEngine>();
        builder.RegisterType<AuthenticationEngine>().As<IAuthenticationEngine>();
        builder.RegisterType<UsersManager>().As<IUsersManager>();
    }
}