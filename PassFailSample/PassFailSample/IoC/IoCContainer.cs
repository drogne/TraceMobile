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

            builder.RegisterType<MainPageMaster>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<MainPageMasterViewModel>().SingleInstance();

            builder.RegisterType<HomeScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<HomeScreenViewModel>().SingleInstance();

            builder.RegisterType<ScanRequestedScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<ScanRequestedScreenViewModel>().SingleInstance();

            builder.RegisterType<BarcodeScannedScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<BarcodeScannedScreenViewModel>().SingleInstance();
            builder.RegisterType<YesNoQuestionScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<YesNoQuestionScreenViewModel>().SingleInstance();

            builder.RegisterType<FailureFeedbackScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<FailureFeedbackScreenViewModel>().SingleInstance();

            builder.RegisterType<CustomInputScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<CustomInputScreenViewModel>().SingleInstance();

            builder.RegisterType<SettingsScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<SettingsScreenViewModel>().SingleInstance();

            builder.RegisterType<PasswordScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<PasswordScreenViewModel>().SingleInstance();

            builder.RegisterType<UserLoginScreen>().AsSelf().As<IViewFor>().SingleInstance();
            builder.RegisterType<UserLoginScreenViewModel>().SingleInstance();

            IoCContainer.Container = builder.Build();
        }
    }
}