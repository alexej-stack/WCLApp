using WCLApp.Common;
using WCLApp.Common.Containers;

namespace Section2;

public class Section2Module(ISectionsContainer sectionsContainer, IAuthClient authClient) : ISectionModule
{
	public void RegisterSections()
	{
		sectionsContainer.RegisterSection(new Section2(authClient));
	}
}