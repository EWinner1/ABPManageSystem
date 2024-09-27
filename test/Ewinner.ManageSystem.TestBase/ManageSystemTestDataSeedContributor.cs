using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Ewinner.ManageSystem;

public class ManageSystemTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
	public Task SeedAsync(DataSeedContext context)
	{
		/* Seed additional test data... */

		return Task.CompletedTask;
	}
}
