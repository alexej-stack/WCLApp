using System.Collections.Generic;
using System.Windows;

namespace WCLApp.Controls
{
	public readonly struct TabDescription<TSection>
	{
		public string Name { get; init; }
		public TSection Section { get; init; }

		public TabDescription(string name, TSection section)
		{
			Name = name;
			Section = section;
		}
	}
}