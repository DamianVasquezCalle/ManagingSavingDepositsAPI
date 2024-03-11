using DB.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class MSDAContext : DbContext
{
	public MSDAContext(DbContextOptions<MSDAContext> options) 
		: base(options)
	{ 
	
	}

	public DbSet<User> Users { get; set; }
	public DbSet<UserRole> UserRoles { get; set; }
}