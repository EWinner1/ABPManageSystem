using Volo.Abp.Modularity;

namespace Ewinner.ManageSystem;

[DependsOn(
	typeof(ManageSystemApplicationModule),
	typeof(ManageSystemDomainTestModule)
)]
public class ManageSystemApplicationTestModule : AbpModule
{

}
