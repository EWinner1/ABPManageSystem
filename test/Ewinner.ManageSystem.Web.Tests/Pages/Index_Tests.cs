using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Ewinner.ManageSystem.Pages;

public class Index_Tests : ManageSystemWebTestBase
{
	[Fact]
	public async Task Welcome_Page()
	{
		var response = await GetResponseAsStringAsync("/");
		response.ShouldNotBeNull();
	}
}
