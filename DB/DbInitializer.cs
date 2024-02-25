using DB.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
	public static class DbInitializer
	{
		public static void Initialize(MSDAContext context)
		{ 
			if (context.Users.Any())
			{
				return;
			}

			var users = new List<User>()
			{
				new User{ Name="Admin Damian", Lastname="Vasquez", Email="admin.damian.vasquez.calle@gmail.com", Role=UserRole.Admin, Password="damian2202" },
				new User{ Name="Manager Damian", Lastname="Vasquez", Email="manager.damian.vasquez.calle@gmail.com", Role=UserRole.Manager, Password="damian2202" },
				new User{ Name="Damian", Lastname="Vasquez", Email="damian.vasquez.calle@gmail.com", Role=UserRole.Regular, Password="damian2202" },
			};
			context.Users.AddRange(users);

			context.SaveChanges();
		}
	}
}
