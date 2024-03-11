using DB.DomainObjects;

namespace DB;

public static class DbInitializer
{
	public static void Initialize(MSDAContext context)
	{
		InitUserRoles(context);
		InitUsers(context);
	}

	public static void InitUserRoles(MSDAContext context)
	{
		if (context.UserRoles.Any()) return;

		var roles = new List<UserRole>()
		{
			new UserRole{ Name="Admin" },
			new UserRole{ Name="Manager" },
			new UserRole{ Name="Regular" },
		};
		context.UserRoles.AddRange(roles);
		context.SaveChanges();
	}

	public static void InitUsers(MSDAContext context)
	{
		if (context.Users.Any()) return;

		var savedRoles = context.UserRoles;

		var adminRole = savedRoles.Single(x => x.Name == "Admin");
		var managerRole = savedRoles.Single(x => x.Name == "Manager");
		var regularRole = savedRoles.Single(x => x.Name == "Regular");

		var defaultPassword = Utils.Utils.EncodeString("damian2202");

		var users = new List<User>()
		{
			new User{ FirstName="Admin", MiddleName="Damian", LastName="Vasquez", Email="admin.damian.vasquez.calle@gmail.com", Password=defaultPassword, UserRoleId=adminRole.Id },
			new User{ FirstName="Manager", MiddleName="Damian", LastName="Vasquez", Email="manager.damian.vasquez.calle@gmail.com", Password=defaultPassword, UserRoleId=managerRole.Id },
			new User{ FirstName="Regular", MiddleName="Damian", LastName="Vasquez", Email="damian.vasquez.calle@gmail.com", Password=defaultPassword, UserRoleId=regularRole.Id },
		};
		context.Users.AddRange(users);
		context.SaveChanges();
	}
}
