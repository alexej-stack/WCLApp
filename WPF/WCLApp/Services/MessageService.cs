using WCLApp.Common;

namespace WCLApp.Services;

public class MessageService : IMessageService
{
	private readonly Dictionary<Type, List<object>> recipients = new Dictionary<Type, List<object>>();

	public void Send<T>(T message)
	{
		var messageType = typeof(T);
		if (recipients.ContainsKey(messageType))
		{
			var actions = recipients[messageType].OfType<Action<T>>().ToList();
			actions.ForEach(action => action?.Invoke(message));
		}
	}

	public void Register<T>(object recipient, Action<T> action)
	{
		var messageType = typeof(T);
		if (!recipients.ContainsKey(messageType))
		{
			recipients[messageType] = new List<object>();
		}

		recipients[messageType].Add(action);
	}

	public void Unregister<T>(object recipient)
	{
		var messageType = typeof(T);
		if (recipients.ContainsKey(messageType))
		{
			recipients[messageType].Remove(recipient);
		}
	}
}