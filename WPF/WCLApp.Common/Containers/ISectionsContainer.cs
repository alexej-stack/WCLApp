using System.Collections.ObjectModel;

namespace WCLApp.Common.Containers;

public interface ISectionsContainer
{
	void RegisterSection(ISection section);
	ReadOnlyObservableCollection<ISection> GetSections();
}