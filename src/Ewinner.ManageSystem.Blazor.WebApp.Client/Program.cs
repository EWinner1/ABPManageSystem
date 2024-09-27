using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;

namespace Ewinner.ManageSystem.Blazor.WebApp.Client;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);
		var application = await builder.AddApplicationAsync<ManageSystemBlazorClientModule>(options =>
		{
			options.UseAutofac();
		});

		var host = builder.Build();

		await application.InitializeApplicationAsync(host.Services);

		await host.RunAsync();
	}
}
