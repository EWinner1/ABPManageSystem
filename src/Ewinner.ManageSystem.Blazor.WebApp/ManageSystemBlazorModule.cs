using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Ewinner.ManageSystem.Blazor.WebApp.Client;
using Ewinner.ManageSystem.Blazor.WebApp.Client.Menus;
using Ewinner.ManageSystem.Blazor.WebApp.Components;
using Ewinner.ManageSystem.EntityFrameworkCore;
using Ewinner.ManageSystem.Localization;
using Ewinner.ManageSystem.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme;
using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Blazor.Server;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Blazor.Server;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Blazor.Server;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Ewinner.ManageSystem.Blazor.WebApp;

[DependsOn(
	typeof(ManageSystemApplicationModule),
	typeof(ManageSystemEntityFrameworkCoreModule),
	typeof(ManageSystemHttpApiModule),
	typeof(AbpAutofacModule),
	typeof(AbpSwashbuckleModule),
	typeof(AbpAspNetCoreSerilogModule),
	typeof(AbpAccountWebOpenIddictModule),
	typeof(AbpAspNetCoreComponentsServerLeptonXLiteThemeModule),
	typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
	typeof(AbpIdentityBlazorServerModule),
	typeof(AbpTenantManagementBlazorServerModule),
	typeof(AbpSettingManagementBlazorServerModule)
   )]
public class ManageSystemBlazorModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
		{
			options.AddAssemblyResource(
				typeof(ManageSystemResource),
				typeof(ManageSystemDomainModule).Assembly,
				typeof(ManageSystemDomainSharedModule).Assembly,
				typeof(ManageSystemApplicationModule).Assembly,
				typeof(ManageSystemApplicationContractsModule).Assembly,
				typeof(ManageSystemBlazorModule).Assembly
			);
		});

		PreConfigure<OpenIddictBuilder>(builder =>
		{
			builder.AddValidation(options =>
			{
				options.AddAudiences("ManageSystem");
				options.UseLocalServer();
				options.UseAspNetCore();
			});
		});

		if (!hostingEnvironment.IsDevelopment())
		{
			PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
			{
				options.AddDevelopmentEncryptionAndSigningCertificate = false;
			});

			PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
			{
				serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "edae68de-3b45-4630-a30a-35e0ebb22b6f");
			});
		}

		PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
		{
			options.IsBlazorWebApp = true;
		});
	}

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		// Add services to the container.
		context.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddInteractiveWebAssemblyComponents();

		ConfigureAuthentication(context);
		ConfigureUrls(configuration);
		ConfigureBundles();
		ConfigureAutoMapper();
		ConfigureVirtualFileSystem(hostingEnvironment);
		ConfigureSwaggerServices(context.Services);
		ConfigureAutoApiControllers();
		ConfigureBlazorise(context);
		ConfigureRouter(context);
		ConfigureMenu(context);
	}

	private void ConfigureAuthentication(ServiceConfigurationContext context)
	{
		context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
		context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
		{
			options.IsDynamicClaimsEnabled = true;
		});
	}

	private void ConfigureUrls(IConfiguration configuration)
	{
		Configure<AppUrlOptions>(options =>
		{
			options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
			options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
		});
	}

	private void ConfigureBundles()
	{
		Configure<AbpBundlingOptions>(options =>
		{
			// MVC UI
			options.StyleBundles.Configure(
				LeptonXLiteThemeBundles.Styles.Global,
				bundle =>
				{
					bundle.AddFiles("/global-styles.css");
				}
			);

			//BLAZOR UI
			options.StyleBundles.Configure(
				BlazorLeptonXLiteThemeBundles.Styles.Global,
				bundle =>
				{
					bundle.AddFiles("/blazor-global-styles.css");
					//You can remove the following line if you don't use Blazor CSS isolation for components
					bundle.AddFiles(new BundleFile("/Ewinner.ManageSystem.Blazor.WebApp.Client.styles.css", true));
				}
			);
		});
	}

	private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
	{
		if (hostingEnvironment.IsDevelopment())
		{
			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.ReplaceEmbeddedByPhysical<ManageSystemDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Ewinner.ManageSystem.Domain.Shared"));
				options.FileSets.ReplaceEmbeddedByPhysical<ManageSystemDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Ewinner.ManageSystem.Domain"));
				options.FileSets.ReplaceEmbeddedByPhysical<ManageSystemApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Ewinner.ManageSystem.Application.Contracts"));
				options.FileSets.ReplaceEmbeddedByPhysical<ManageSystemApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Ewinner.ManageSystem.Application"));
				options.FileSets.ReplaceEmbeddedByPhysical<ManageSystemBlazorModule>(hostingEnvironment.ContentRootPath);
			});
		}
	}

	private void ConfigureSwaggerServices(IServiceCollection services)
	{
		services.AddAbpSwaggerGen(
			options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "ManageSystem API", Version = "v1" });
				options.DocInclusionPredicate((docName, description) => true);
				options.CustomSchemaIds(type => type.FullName);
			}
		);
	}

	private void ConfigureBlazorise(ServiceConfigurationContext context)
	{
		context.Services
			.AddBootstrap5Providers()
			.AddFontAwesomeIcons();
	}

	private void ConfigureMenu(ServiceConfigurationContext context)
	{
		Configure<AbpNavigationOptions>(options =>
		{
			options.MenuContributors.Add(new ManageSystemMenuContributor(context.Services.GetConfiguration()));
		});
	}

	private void ConfigureRouter(ServiceConfigurationContext context)
	{
		Configure<AbpRouterOptions>(options =>
		{
			options.AppAssembly = typeof(ManageSystemBlazorModule).Assembly;
			options.AdditionalAssemblies.Add(typeof(ManageSystemBlazorClientModule).Assembly);
		});
	}

	private void ConfigureAutoApiControllers()
	{
		Configure<AbpAspNetCoreMvcOptions>(options =>
		{
			options.ConventionalControllers.Create(typeof(ManageSystemApplicationModule).Assembly);
		});
	}

	private void ConfigureAutoMapper()
	{
		Configure<AbpAutoMapperOptions>(options =>
		{
			options.AddMaps<ManageSystemBlazorModule>();
		});
	}

	public override void OnApplicationInitialization(ApplicationInitializationContext context)
	{
		var env = context.GetEnvironment();
		var app = context.GetApplicationBuilder();

		app.UseAbpRequestLocalization();

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseCorrelationId();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseAuthentication();
		app.UseAbpOpenIddictValidation();

		if (MultiTenancyConsts.IsEnabled)
		{
			app.UseMultiTenancy();
		}
		app.UseUnitOfWork();
		app.UseDynamicClaims();
		app.UseAntiforgery();
		app.UseAuthorization();

		app.UseSwagger();
		app.UseAbpSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "ManageSystem API");
		});

		app.UseConfiguredEndpoints(builder =>
		{
			builder.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode()
				.AddInteractiveWebAssemblyRenderMode()
				.AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AbpRouterOptions>>().Value.AdditionalAssemblies.ToArray());
		});
	}
}
