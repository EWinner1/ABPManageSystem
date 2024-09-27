using System.Threading.Tasks;

namespace Ewinner.ManageSystem.Data;

public interface IManageSystemDbSchemaMigrator
{
	Task MigrateAsync();
}
