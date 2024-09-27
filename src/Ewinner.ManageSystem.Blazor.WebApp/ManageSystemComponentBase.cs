﻿using Ewinner.ManageSystem.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Ewinner.ManageSystem.Blazor.WebApp;

public abstract class ManageSystemComponentBase : AbpComponentBase
{
	protected ManageSystemComponentBase()
	{
		LocalizationResource = typeof(ManageSystemResource);
	}
}