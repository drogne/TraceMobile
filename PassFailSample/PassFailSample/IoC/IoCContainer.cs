using Autofac;
using PassFailSample.Helpers;
using PassFailSample.Models;
using PassFailSample.Utilities;
using PassFailSample.Utilities.Navigation;
using PassFailSample.ViewModels;
using PassFailSample.Views;

namespace PassFailSample.IoC
{
    public class IoCContainer
    {
        public static IContainer Container { get; set; }

        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CredentialsService>().SingleInstance();
            builder.RegisterType<NavigationService>().SingleInstance();
            builder.RegisterType<DataStorage>().SingleInstance();
            builder.RegisterType<IdleTimeoutTimer>().SingleInstance();
            builder.RegisterType<Settings>().SingleInstance();
            builder.RegisterType<Session>().SingleInstance();

            builder.RegisterType<MainPageMaster>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<MainPageMasterViewModel>().SingleInstance();

            builder.RegisterType<HomeScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<HomeScreenViewModel>().SingleInstance();

            builder.RegisterType<SettingsScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<SettingsScreenViewModel>().SingleInstance();

            builder.RegisterType<PasswordScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<PasswordScreenViewModel>().SingleInstance();

            builder.RegisterType<UserLoginScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<UserLoginScreenViewModel>().SingleInstance();

            builder.RegisterType<OrderScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<OrderViewModel>().SingleInstance();

            builder.RegisterType<ComponentScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<ComponentViewModel>().SingleInstance();

            builder.RegisterType<ResultsScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<ResultsScreenViewModel>().SingleInstance();

            IoCContainer.Container = builder.Build();
        }
    }
}