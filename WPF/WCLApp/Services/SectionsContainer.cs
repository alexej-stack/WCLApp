using System.Collections.ObjectModel;
using WCLApp.Common;
using WCLApp.Common.Containers;

namespace WCLApp.Services;

public class SectionsContainer : ISectionsContainer
{
	private readonly ObservableCollection<ISection> sections;

	public SectionsContainer()
	{
		sections = new ObservableCollection<ISection>();
	}

	public void RegisterSection(ISection section)
	{
		if (section == null)
		{
			throw new ArgumentNullException(nameof(section));
		}

		sections.Add(section);
	}

	public ReadOnlyObservableCollection<ISection> GetSections()
	{
		return new ReadOnlyObservableCollection<ISection>(sections);
	}
}