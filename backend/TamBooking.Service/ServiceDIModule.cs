using Autofac;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class ServiceDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BandService>().As<IBandService>();
            builder.RegisterType<GigTypeService>().As<IGigTypeService>();
            builder.RegisterType<CountyService>().As<ICountyService>();
            builder.RegisterType<TownService>().As<ITownService>();
            builder.RegisterType<BandMemberService>().As<IBandMemberService>();
            builder.RegisterType<GigService>().As<IGigService>();
            builder.RegisterType<AddressService>().As<IAddressService>();
            builder.RegisterType<ReviewService>().As<IReviewService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<JwtService>().As<IJwtService>();
            builder.RegisterType<ClientService>().As<IClientService>();
            builder.RegisterType<EmailService>().As<IEmailService>();
            builder.RegisterType<RecepientTypeService>().As<IRecepientTypeService>();
            builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
        }
    }
}