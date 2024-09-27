using Volo.Abp.Modularity;

namespace Ewinner.ManageSystem;

[DependsOn(
	typeof(ManageSystemDomainModule),
	typeof(ManageSystemTestBaseModule)
)]
public class ManageSystemDomainTestModule : AbpModule
{

}
