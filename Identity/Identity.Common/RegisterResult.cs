namespace Identity.Service.Common;

public class RegisterResult(bool status, IEnumerable<String> errors)
{
	public bool Status { get; } = status;
	public IEnumerable<String> Errors { get; } = errors;
}