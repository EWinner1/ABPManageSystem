﻿using Ewinner.ManageSystem.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Ewinner.ManageSystem.Web;

[Dependency(ReplaceServices = true)]
public class ManageSystemBrandingProvider : DefaultBrandingProvider
{
	private IStringLocalizer<ManageSystemResource> _localizer;

	public ManageSystemBrandingProvider(IStringLocalizer<ManageSystemResource> localizer)
	{
		_localizer = localizer;
	}

	public override string AppName => _localizer["AppName"];
}