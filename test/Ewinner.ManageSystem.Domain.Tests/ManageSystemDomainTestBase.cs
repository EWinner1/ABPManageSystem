using Volo.Abp.Modularity;

namespace Ewinner.ManageSystem;

/* Inherit from this class for your domain layer tests. */
public abstract class ManageSystemDomainTestBase<TStartupModule> : ManageSystemTestBase<TStartupModule>
	where TStartupModule : IAbpModule
{

}
