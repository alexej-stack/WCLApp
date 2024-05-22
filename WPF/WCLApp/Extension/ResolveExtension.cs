using System.Windows.Markup;

namespace WCLApp.Extension;

public class ResolveExtension : MarkupExtension
{
	public Type Type { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (Type == null)
		{
			throw new InvalidOperationException("Type property must be set.");
		}

		return ServiceSetupManager.Resolve(Type);
	}
}