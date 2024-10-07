using Autofac;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class RepositoryDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BandRepository>().As<IBandRepository>();
            builder.RegisterType<GigTypeRepository>().As<IGigTypeRepository>();
            builder.RegisterType<CountyRepository>().As<ICountyRepository>();
            builder.RegisterType<TownRepository>().As<ITownRepository>();
            builder.RegisterType<BandMemberRepository>().As<IBandMemberRepository>();
            builder.RegisterType<GigRepository>().As<IGigRepository>();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<ClientRepository>().As<IClientRepository>();
            builder.RegisterType<RecepientTypeRepository>().As<IRecepientTypeRepository>();
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
        }
    }
}