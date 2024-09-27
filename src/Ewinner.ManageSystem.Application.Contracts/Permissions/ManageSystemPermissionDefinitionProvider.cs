using Ewinner.ManageSystem.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Ewinner.ManageSystem.Permissions;

public class ManageSystemPermissionDefinitionProvider : PermissionDefinitionProvider
{
	public override void Define(IPermissionDefinitionContext context)
	{
		var myGroup = context.AddGroup(ManageSystemPermissions.GroupName);
		//Define your own permissions here. Example:
		//myGroup.AddPermission(ManageSystemPermissions.MyPermission1, L("Permission:MyPermission1"));
	}

	private static LocalizableString L(string name)
	{
		return LocalizableString.Create<ManageSystemResource>(name);
	}
}
