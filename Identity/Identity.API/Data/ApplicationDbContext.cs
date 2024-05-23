using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public DbSet<ApplicationUser> Users { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		//modelBuilder.Entity<ApplicationUser>()
		//	.ToTable("Users")
		//	.HasKey(u => u.Id);
	}
}

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
	public void Configure(EntityTypeBuilder<IdentityRole> builder)
	{
		builder.HasData(
			new IdentityRole
			{
				Name = "Manager",
				NormalizedName = "MANAGER"
			},
			new IdentityRole
			{
				Name = "Administrator",
				NormalizedName = "ADMINISTRATOR"
			}
		);
	}
}