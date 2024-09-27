using Volo.Abp.Settings;

namespace Ewinner.ManageSystem.Settings;

public class ManageSystemSettingDefinitionProvider : SettingDefinitionProvider
{
	public override void Define(ISettingDefinitionContext context)
	{
		//Define your own settings here. Example:
		//context.Add(new SettingDefinition(ManageSystemSettings.MySetting1));
	}
}
