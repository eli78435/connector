using Autofac;
using Edc.Identity.Utilities;
using Edc.Identity.WebApi.Settings;
using Edc.Identity.WebApi.Utilities;
using Microsoft.Extensions.Options;

namespace Edc.Identity.WebApi;

public class IdentityWebApiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserNameAndPasswordHashPasswordAdapter>().As<IUserNameAndPasswordHashValidator>();
        builder.Register<IUserTokenGenerator>(ctx =>
        {
            var settings = ctx.Resolve<IOptions<JwtSettings>>().Value;
            return new JwtUserTokenGenerator(settings.Key, settings.Issuer, settings.Audience);
        }).SingleInstance();
    }
}