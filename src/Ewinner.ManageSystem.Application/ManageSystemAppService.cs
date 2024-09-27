using Ewinner.ManageSystem.Localization;
using Volo.Abp.Application.Services;

namespace Ewinner.ManageSystem;

/* Inherit your application services from this class.
 */
public abstract class ManageSystemAppService : ApplicationService
{
	protected ManageSystemAppService()
	{
		LocalizationResource = typeof(ManageSystemResource);
	}
}
