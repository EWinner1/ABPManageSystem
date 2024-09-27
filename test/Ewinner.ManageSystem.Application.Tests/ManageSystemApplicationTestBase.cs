using Volo.Abp.Modularity;

namespace Ewinner.ManageSystem;

public abstract class ManageSystemApplicationTestBase<TStartupModule> : ManageSystemTestBase<TStartupModule>
	where TStartupModule : IAbpModule
{

}
