using Autofac;
using Edc.Identity.Accessors;
using Edc.Identity.Infrastructure.MongoDb;
using Edc.Identity.Infrastructure.MongoDb.Dal;
using Edc.Identity.Infrastructure.MongoDb.Repositories;
using Edc.Identity.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;

namespace Edc.Identity.Infrastructure;

public class IdentityInfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MongoIdGenerator>().As<Edc.Common.IIdGenerator>();
        
        builder.Register<IMongoCollectionHolder<UserDal>>(ctx =>
        {
            var mongoClientHolder = ctx.Resolve<IMongoClientHolder>();
            var usersSettings = ctx.Resolve<IOptionsSnapshot<MongoUsersSettings>>().Value;
            return new UserCollectionHolder(mongoClientHolder.Client,
                usersSettings.Database,
                usersSettings.Collection);
        }).SingleInstance();
        
        builder.RegisterType<MongoUsersRepository>().As<IUsersRepository>();
    }
}