using System.IO;
using System.Reflection;
using System.Windows;
using WCLApp.Auth;
using WCLApp.Common;
using WCLApp.Common.Containers;
using WCLApp.Extension;
using WCLApp.Services;

namespace WCLApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private void OnStartup(object sender, StartupEventArgs e)
	{
		RegisterServices();
		RegisterViews();
		ServiceSetupManager.Startup();
		var window = new MainWindow
		{
			Width = 1000,
			Height = 600,
			WindowStartupLocation = WindowStartupLocation.CenterScreen
		};

		window.DataContext = new MainViewModel(
			ServiceSetupManager.GetService<IAuthClient>(),
			ServiceSetupManager.GetService<ISectionsContainer>()
		);
		var appSettings = ServiceSetupManager.GetService<AppSettings>();
		LoadSections(appSettings.SectionsStorage);
		window.Show();
	}

	private void RegisterViews()
	{
		ServiceSetupManager.RegisterViewWithViewModel<AuthView, AuthViewModel>();
		ServiceSetupManager.RegisterViewWithViewModel<RegisterView, RegisterViewModel>();
	}

	private static void RegisterServices()
	{
		ServiceSetupManager.RegisterService<ISectionsContainer, SectionsContainer>();
		ServiceSetupManager.RegisterService<IAuthClient, AuthClient>();
		ServiceSetupManager.RegisterService<IDialogService, DialogService>();
		ServiceSetupManager.RegisterService<IMessageService, MessageService>();
		ServiceSetupManager.RegisterAppSettings("Config\\AppSettings.json");
	}

	private void LoadSections(string sectionsPath)
	{
		var assemblyNames = Directory.GetFiles(sectionsPath, "*.dll");
		foreach (var assemblyName in assemblyNames)
		{
			var assembly = Assembly.LoadFrom(assemblyName);
			var moduleTypes = assembly.GetTypes().Where(t =>
				typeof(ISectionModule).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

			foreach (var moduleType in moduleTypes)
			{
				var module = ServiceSetupManager.Resolve<ISectionModule>(moduleType);
				module?.RegisterSections();
			}
		}
	}
}