using Ewinner.ManageSystem.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Ewinner.ManageSystem.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ManageSystemController : AbpControllerBase
{
	protected ManageSystemController()
	{
		LocalizationResource = typeof(ManageSystemResource);
	}
}
