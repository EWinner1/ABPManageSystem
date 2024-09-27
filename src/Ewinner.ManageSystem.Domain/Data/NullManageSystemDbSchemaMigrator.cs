using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Ewinner.ManageSystem.Data;

/* This is used if database provider does't define
 * IManageSystemDbSchemaMigrator implementation.
 */
public class NullManageSystemDbSchemaMigrator : IManageSystemDbSchemaMigrator, ITransientDependency
{
	public Task MigrateAsync()
	{
		return Task.CompletedTask;
	}
}
