using Ewinner.ManageSystem.Localization;
using Ewinner.ManageSystem.MultiTenancy;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace Ewinner.ManageSystem.Blazor.WebApp.Tiered.Client.Menus;

public class ManageSystemMenuContributor : IMenuContributor
{
	private readonly IConfiguration _configuration;

	public ManageSystemMenuContributor(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task ConfigureMenuAsync(MenuConfigurationContext context)
	{
		if (context.Menu.Name == StandardMenus.Main)
		{
			await ConfigureMainMenuAsync(context);
		}
		else if (context.Menu.Name == StandardMenus.User)
		{
			await ConfigureUserMenuAsync(context);
		}
	}

	private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
	{
		var l = context.GetLocalizer<ManageSystemResource>();

		context.Menu.Items.Insert(
			0,
			new ApplicationMenuItem(
				ManageSystemMenus.Home,
				l["Menu:Home"],
				"/",
				icon: "fas fa-home"
			)
		);

		var administration = context.Menu.GetAdministration();

		if (MultiTenancyConsts.IsEnabled)
		{
			administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
		}
		else
		{
			administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
		}

		administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
		administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

		return Task.CompletedTask;
	}

	private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
	{
		if (!OperatingSystem.IsBrowser())
		{
			return Task.CompletedTask;
		}

		var authServerUrl = _configuration["AuthServer:Authority"] ?? "";
		var accountStringLocalizer = context.GetLocalizer<AccountResource>();

		context.Menu.AddItem(new ApplicationMenuItem(
				"Account.Manage",
				accountStringLocalizer["MyAccount"],
				$"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
				icon: "fa fa-cog",
				order: 1000,
				target: "_blank")
			.RequireAuthenticated());

		return Task.CompletedTask;
	}
}
