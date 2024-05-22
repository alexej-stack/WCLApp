namespace WCLApp.Common;

public interface IMessageService
{
	void Send<T>(T message);
	void Register<T>(object recipient, Action<T> action);
	void Unregister<T>(object recipient);
}