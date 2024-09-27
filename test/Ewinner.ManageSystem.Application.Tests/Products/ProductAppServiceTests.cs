using Ewinner.ManageSystem.Interfaces.IServices;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;

namespace Ewinner.ManageSystem.Products
{

	public abstract class ProductAppServiceTests<TStartupModule> : ManageSystemApplicationTestBase<TStartupModule>
		where TStartupModule : IAbpModule
	{
		private readonly IProductAppService productAppService;
		private readonly PagedAndSortedResultRequestDto dto;

		public ProductAppServiceTests()
		{
			productAppService = GetRequiredService<IProductAppService>();
			dto = new PagedAndSortedResultRequestDto();
		}

		[Fact]
		public async Task Should_Get_Products_List()
		{
			var result = await productAppService.GetListAsync(dto);

			result.TotalCount.ShouldBe(4);
			result.Items.ShouldContain(x => x.Name.Contains("Xiaomi 13 Ultra black"));

			Console.WriteLine("ShouldGetProductList Pass");
		}
	}
}
