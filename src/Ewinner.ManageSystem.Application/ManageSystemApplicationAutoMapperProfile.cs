using AutoMapper;
using Ewinner.ManageSystem.DTOs;
using Ewinner.ManageSystem.Models;

namespace Ewinner.ManageSystem;

public class ManageSystemApplicationAutoMapperProfile : Profile
{
	public ManageSystemApplicationAutoMapperProfile()
	{
		/* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

		CreateMap<Product, ProductDto>();
		CreateMap<CreateOrUpdateProductDto, Product>();
		CreateMap<Category, CategoryLookupDto>();
	}
}
