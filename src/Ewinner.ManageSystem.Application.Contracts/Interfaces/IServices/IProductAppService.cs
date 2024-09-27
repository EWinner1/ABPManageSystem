using Ewinner.ManageSystem.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ewinner.ManageSystem.Interfaces.IServices
{
	public interface IProductAppService : IApplicationService
	{
		Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input);
		Task CreateAsync(CreateOrUpdateProductDto input);
		Task<ListResultDto<CategoryLookupDto>> GetCategoriesAsync();
	}
}
