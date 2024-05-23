namespace Identity.Service.Common;

public class LoginResult(bool status)
{
	public bool Status { get; } = status;
}