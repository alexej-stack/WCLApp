using Identity.API.Data;
using Identity.API.Extension;
using Identity.API.Services;
using Identity.Service.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
	var kestrelSection = builder.Configuration.GetSection("Kestrel");
	serverOptions.Configure(kestrelSection);
});

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
	var connectionString = builder.Configuration.GetConnectionString("IdentityDbConnection");
	//opts.UseSqlServer(connectionString,
	//	sqlServerOptions => sqlServerOptions.MigrationsAssembly(typeof(Program).Assembly.FullName));
	//ToDO: add possibility to define sql server provider
	opts.UseSqlite(connectionString,
		options => { options.MigrationsAssembly(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name); });
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();