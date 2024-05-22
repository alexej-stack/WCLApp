using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WCLApp.Extension;

internal class ServiceSetupManager
{
	private static readonly ServiceCollection services = new();
	private static ServiceProvider serviceProvider;

	public static ServiceProvider Startup()
	{
		serviceProvider = services.BuildServiceProvider();
		return serviceProvider;
	}

	public static void RegisterService<TServiceInterface, TServiceImpl>()
		where TServiceInterface : class
		where TServiceImpl : class, TServiceInterface
	{
		services.AddSingleton<TServiceInterface, TServiceImpl>();
	}

	public static void RegisterAppSettings(string settingsFile)
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile(settingsFile, optional: false, reloadOnChange: true);

		IConfigurationRoot configuration = builder.Build();
		var appSettingsSection = configuration.GetSection("AppSettings");
		var authUriString = appSettingsSection["AuthUri"];
		var sectionsStorage = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Sections";

		if (Uri.TryCreate(authUriString, UriKind.Absolute, out Uri authUri))
		{
			var appSettings = new AppSettings(authUri, sectionsStorage);
			services.AddSingleton(appSettings);
		}
	}

	public static TService GetService<TService>()
	{
		var service = serviceProvider.GetService<TService>();
		if (service == null)
		{
			return (TService)Activator.CreateInstance(typeof(TService)) ??
			       throw new InvalidOperationException("Object of this type can't be created");
		}

		return service;
	}

	public static void RegisterViewWithViewModel<TView, TViewModel>()
		where TView : FrameworkElement
		where TViewModel : class
	{
		services.AddTransient<TViewModel>();
		services.AddTransient(provider =>
		{
			var viewModel = provider.GetRequiredService<TViewModel>();
			var viewType = typeof(TView);
			var constructor = viewType.GetConstructors().First();
			var parameters = constructor.GetParameters()
				.Select(p => provider.GetRequiredService(p.ParameterType))
				.ToArray();

			var view = (TView)Activator.CreateInstance(viewType, parameters);
			if (view != null)
			{
				view.DataContext = viewModel;
			}

			return view;
		});
	}

	public static TType Resolve<TType>(Type type)
	{
		return (TType)ActivatorUtilities.CreateInstance(serviceProvider, type);
	}

	public static object Resolve(Type type)
	{
		return serviceProvider.GetService(type);
	}
}