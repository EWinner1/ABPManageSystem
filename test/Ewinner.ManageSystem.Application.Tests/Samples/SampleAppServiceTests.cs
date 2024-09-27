using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Xunit;

namespace Ewinner.ManageSystem.Samples;

/* This is just an example test class.
 * Normally, you don't test code of the modules you are using
 * (like IIdentityUserAppService here).
 * Only test your own application services.
 */
public abstract class SampleAppServiceTests<TStartupModule> : ManageSystemApplicationTestBase<TStartupModule>
	where TStartupModule : IAbpModule
{
	private readonly IIdentityUserAppService _userAppService;
	// private readonly IProductAppService productAppService;
	// private readonly PagedAndSortedResultRequestDto dto;

	public SampleAppServiceTests()
	{
		_userAppService = GetRequiredService<IIdentityUserAppService>();
		// productAppService = GetRequiredService<IProductAppService>();
		// dto = new PagedAndSortedResultRequestDto();
	}

	[Fact]
	public async Task Initial_Data_Should_Contain_Admin_User()
	{
		//Act
		var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());

		//Assert
		result.TotalCount.ShouldBeGreaterThan(0);
		result.Items.ShouldContain(u => u.UserName == "admin");
	}

	/*
	[Fact]
	public async Task Should_Get_Products_List()
	{
		var result = await productAppService.GetListAsync(dto);

		result.TotalCount.ShouldBe(4);
		result.Items.ShouldContain(x => x.Name.Contains("Xiaomi 13 Ultra black"));

		Console.WriteLine("ShouldGetProductList Pass");
	}
	*/
}
