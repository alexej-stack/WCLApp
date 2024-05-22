namespace WCLApp;

public class AppSettings(Uri uri, string sectionsStorage)
{
	public Uri AuthUri { get; internal set; } = uri;
	public string SectionsStorage { get; internal set; } = sectionsStorage;
}