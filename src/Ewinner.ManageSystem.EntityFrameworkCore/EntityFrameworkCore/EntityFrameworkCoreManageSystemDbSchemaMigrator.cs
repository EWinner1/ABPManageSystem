﻿using Ewinner.ManageSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Ewinner.ManageSystem.EntityFrameworkCore;

public class EntityFrameworkCoreManageSystemDbSchemaMigrator
	: IManageSystemDbSchemaMigrator, ITransientDependency
{
	private readonly IServiceProvider _serviceProvider;

	public EntityFrameworkCoreManageSystemDbSchemaMigrator(
		IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task MigrateAsync()
	{
		/* We intentionally resolve the ManageSystemDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

		await _serviceProvider
			.GetRequiredService<ManageSystemDbContext>()
			.Database
			.MigrateAsync();
	}
}
