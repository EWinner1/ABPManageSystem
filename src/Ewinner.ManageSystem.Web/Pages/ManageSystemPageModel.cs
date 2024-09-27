using Ewinner.ManageSystem.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Ewinner.ManageSystem.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ManageSystemPageModel : AbpPageModel
{
	protected ManageSystemPageModel()
	{
		LocalizationResourceType = typeof(ManageSystemResource);
	}
}
