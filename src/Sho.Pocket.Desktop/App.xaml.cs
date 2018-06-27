using System.Configuration;
using Sho.Pocket.BLL.Services;
using Sho.Pocket.Core.Abstractions;
using System.Windows;
using Unity;
using Sho.Pocket.Desktop.Configuration;
using Unity.Injection;

namespace Sho.Pocket.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PocketLocalDbConnection"].ConnectionString;

            IUnityContainer container = new UnityContainer();


            container.RegisterType<IDbConfiguration, DbConfiguration>(new InjectionConstructor(connectionString));
            container.RegisterType<ISummaryService, SummaryService>();


            Dashboard dashboard = container.Resolve<Dashboard>();

            dashboard.Show();
        }
    }
}
