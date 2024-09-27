using Ewinner.ManageSystem.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Ewinner.ManageSystem;

[DependsOn(
	typeof(AbpAuditLoggingDomainSharedModule),
	typeof(AbpBackgroundJobsDomainSharedModule),
	typeof(AbpFeatureManagementDomainSharedModule),
	typeof(AbpIdentityDomainSharedModule),
	typeof(AbpOpenIddictDomainSharedModule),
	typeof(AbpPermissionManagementDomainSharedModule),
	typeof(AbpSettingManagementDomainSharedModule),
	typeof(AbpTenantManagementDomainSharedModule)
	)]
public class ManageSystemDomainSharedModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
	{
		ManageSystemGlobalFeatureConfigurator.Configure();
		ManageSystemModuleExtensionConfigurator.Configure();
	}

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpVirtualFileSystemOptions>(options =>
		{
			options.FileSets.AddEmbedded<ManageSystemDomainSharedModule>();
		});

		Configure<AbpLocalizationOptions>(options =>
		{
			options.Resources
				.Add<ManageSystemResource>("en")
				.AddBaseTypes(typeof(AbpValidationResource))
				.AddVirtualJson("/Localization/ManageSystem");

			options.DefaultResourceType = typeof(ManageSystemResource);
		});

		Configure<AbpExceptionLocalizationOptions>(options =>
		{
			options.MapCodeNamespace("ManageSystem", typeof(ManageSystemResource));
		});
	}
}
