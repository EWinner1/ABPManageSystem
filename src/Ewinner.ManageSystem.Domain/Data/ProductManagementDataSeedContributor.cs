using Ewinner.ManageSystem.Consts;
using Ewinner.ManageSystem.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Ewinner.ManageSystem.Data
{
	class ProductManagementDataSeedContributor(
		IRepository<Category, Guid> categoryRepository,
		IRepository<Product, Guid> productRepository) : IDataSeedContributor, ITransientDependency
	{
		private readonly IRepository<Category, Guid> categoryRepository = categoryRepository;
		private readonly IRepository<Product, Guid> productRepository = productRepository;

		public async Task SeedAsync(DataSeedContext context)
		{
			if (await categoryRepository.CountAsync() > 0)
			{
				return;
			}

			var phones = new Category { Name = "Phone" };
			var pads = new Category { Name = "Pad" };

			await categoryRepository.InsertManyAsync([phones, pads]);

			var phone1 = new Product
			{
				Category = phones,
				Name = "Xiaomi 13 Ultra black",
				Price = 7999,
				ReleaseDate = new DateTime(2023, 2, 17),
				StockState = ProductStockState.InStock
			};

			var phone2 = new Product
			{
				Category = phones,
				Name = "iPhone 13 Pro Plus Max",
				Price = 8999,
				ReleaseDate = new DateTime(2025, 5, 22),
				StockState = ProductStockState.PreOrder
			};

			var pad1 = new Product
			{
				Category = pads,
				Name = "iPad Pro A1980",
				Price = 9999,
				ReleaseDate = new DateTime(2018, 1, 8),
				StockState = ProductStockState.Stopped
			};

			var pad2 = new Product
			{
				Category = pads,
				Name = "Xiaomi Pad 6 Pro",
				Price = 1999,
				ReleaseDate = new DateTime(2023, 4, 21),
				StockState = ProductStockState.Stopped
			};

			await productRepository.InsertManyAsync([phone1, phone2, pad1, pad2]);
		}
	}
}
