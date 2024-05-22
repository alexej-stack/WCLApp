using System.Collections.ObjectModel;
using WCLApp.Common;
using WCLApp.Common.Containers;

namespace WCLApp;

public class MainViewModel : ViewModelBase
{
	private readonly ISectionsContainer sectionsContainer;
	private readonly IAuthClient authClient;
	private bool isLogged;

	public bool IsLogged
	{
		get => isLogged;
		private set => SetProperty(ref isLogged, value);
	}

	public ReadOnlyObservableCollection<ISection> Sections => sectionsContainer.GetSections();

	public MainViewModel(IAuthClient authClient, ISectionsContainer sectionsContainer)
	{
		this.authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
		this.sectionsContainer = sectionsContainer ?? throw new ArgumentNullException(nameof(sectionsContainer));

		this.authClient.AuthStatusChanged += OnAuthStatusChanged;
	}

	private void OnAuthStatusChanged(object sender, AuthStateChangedEventArgs e)
	{
		IsLogged = e.IsLogged;
	}
}