using WCLApp.Common;
using WCLApp.Common.Containers;

namespace Section1;

public class Section1Module(ISectionsContainer sectionsContainer, IAuthClient authClient) : ISectionModule
{
	public void RegisterSections()
	{
		sectionsContainer.RegisterSection(new Section1(authClient));
	}
}