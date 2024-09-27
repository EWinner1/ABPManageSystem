using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ewinner.ManageSystem.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ManageSystemDbContextFactory : IDesignTimeDbContextFactory<ManageSystemDbContext>
{
	public ManageSystemDbContext CreateDbContext(string[] args)
	{
		ManageSystemEfCoreEntityExtensionMappings.Configure();

		var configuration = BuildConfiguration();

		var builder = new DbContextOptionsBuilder<ManageSystemDbContext>()
			.UseSqlServer(configuration.GetConnectionString("Default"));

		return new ManageSystemDbContext(builder.Options);
	}

	private static IConfigurationRoot BuildConfiguration()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Ewinner.ManageSystem.DbMigrator/"))
			.AddJsonFile("appsettings.json", optional: false);

		return builder.Build();
	}
}
