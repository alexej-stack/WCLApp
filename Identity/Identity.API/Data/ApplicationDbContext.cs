namespace Identity.API.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
	public DbSet<ApplicationUser> Users { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ApplicationUser>()
			.ToTable("Users")
			.HasKey(u => u.Id);
	}
}