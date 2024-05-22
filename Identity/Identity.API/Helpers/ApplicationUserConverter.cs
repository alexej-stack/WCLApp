using Identity.API.Data;
using Identity.Common;

namespace Identity.API.Helpers;

public static class ApplicationUserConverter
{
	public static UserData? GetUserData(this ApplicationUser user)
	{
		if (user != null)
		{
			return new UserData(user.UserName, user.LastName, user.PhoneNumber, user.Email)
			{
				Id = user.Id
			};
		}

		return null;
	}
}