using Ewinner.ManageSystem.EntityFrameworkCore;
using Ewinner.ManageSystem.Localization;
using Ewinner.ManageSystem.MultiTenancy;
using Ewinner.ManageSystem.Web.Menus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Ewinner.ManageSystem.Web;

[DependsOn(
	typeof(ManageSystemHttpApiModule),
	typeof(ManageSystemApplicationModule),
	typeof(ManageSystemEntityFrameworkCoreModule),
	typeof(AbpAutofacModule),
	typeof(AbpIdentityWebModule),
	typeof(AbpSettingManagementWebModule),
	typeof(AbpAccountWebOpenIddictModule),
	typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
	typeof(AbpTenantManagementWebModule),
	typeof(AbpAspNetCoreSerilogModule),
	typeof(AbpSwashbuckleModule)
	)]
public class ManageSystemWebModule : AbpModule
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
				typeof(ManageSystemWebModule).Assembly
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
	}

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		ConfigureAuthentication(context);
		ConfigureUrls(configuration);
		ConfigureBundles();
		ConfigureAutoMapper();
		ConfigureVirtualFileSystem(hostingEnvironment);
		ConfigureNavigationServices();
		ConfigureAutoApiControllers();
		ConfigureSwaggerServices(context.Services);
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
		});
	}

	private void ConfigureBundles()
	{
		Configure<AbpBundlingOptions>(options =>
		{
			options.StyleBundles.Configure(
				LeptonXLiteThemeBundles.Styles.Global,
				bundle =>
				{
					bundle.AddFiles("/global-styles.css");
				}
			);
		});
	}

	private void ConfigureAutoMapper()
	{
		Configure<AbpAutoMapperOptions>(options =>
		{
			options.AddMaps<ManageSystemWebModule>();
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
				options.FileSets.ReplaceEmbeddedByPhysical<ManageSystemWebModule>(hostingEnvironment.ContentRootPath);
			});
		}
	}

	private void ConfigureNavigationServices()
	{
		Configure<AbpNavigationOptions>(options =>
		{
			options.MenuContributors.Add(new ManageSystemMenuContributor());
		});
	}

	private void ConfigureAutoApiControllers()
	{
		Configure<AbpAspNetCoreMvcOptions>(options =>
		{
			options.ConventionalControllers.Create(typeof(ManageSystemApplicationModule).Assembly);
		});
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

	public override void OnApplicationInitialization(ApplicationInitializationContext context)
	{
		var app = context.GetApplicationBuilder();
		var env = context.GetEnvironment();

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseAbpRequestLocalization();

		if (!env.IsDevelopment())
		{
			app.UseErrorPage();
		}

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
		app.UseAuthorization();

		app.UseSwagger();
		app.UseAbpSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "ManageSystem API");
		});

		app.UseAuditing();
		app.UseAbpSerilogEnrichers();
		app.UseConfiguredEndpoints();
	}
}
