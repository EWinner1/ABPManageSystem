using Ewinner.ManageSystem.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Ewinner.ManageSystem.DbMigrator;

[DependsOn(
	typeof(AbpAutofacModule),
	typeof(ManageSystemEntityFrameworkCoreModule),
	typeof(ManageSystemApplicationContractsModule)
	)]
public class ManageSystemDbMigratorModule : AbpModule
{
}
