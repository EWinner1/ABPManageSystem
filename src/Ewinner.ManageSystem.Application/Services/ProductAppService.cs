using Ewinner.ManageSystem.DTOs;
using Ewinner.ManageSystem.Interfaces.IServices;
using Ewinner.ManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Ewinner.ManageSystem.Services
{
	public class ProductAppService(
		IRepository<Product, Guid> productRepository,
		IRepository<Category, Guid> categoryRepository,
		IUnitOfWorkManager unitOfWorkManager) : ManageSystemAppService, IProductAppService
	{
		private readonly IRepository<Product, Guid> productRepository = productRepository;
		private readonly IRepository<Category, Guid> categoryRepository = categoryRepository;
		private readonly IUnitOfWorkManager unitOfWorkManager = unitOfWorkManager;

		public async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
		{
			var queryable = await productRepository.WithDetailsAsync(x => x.Category);
			queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting ?? nameof(Product.Name));

			var products = await AsyncExecuter.ToListAsync(queryable);
			var count = await productRepository.GetCountAsync();

			return new PagedResultDto<ProductDto>(
				count,
				ObjectMapper.Map<List<Product>, List<ProductDto>>(products));
		}

		public async Task CreateAsync(CreateOrUpdateProductDto input)
		{
			var product = ObjectMapper.Map<CreateOrUpdateProductDto, Product>(input);
			await productRepository.InsertAsync(product);
		}

		public async Task<ListResultDto<CategoryLookupDto>> GetCategoriesAsync()
		{
			var categories = await categoryRepository.GetListAsync();
			var categoryLookupDtos = ObjectMapper.Map<List<Category>, List<CategoryLookupDto>>(categories);

			return new ListResultDto<CategoryLookupDto>(categoryLookupDtos);
		}

		/*
		public async Task<ProductDto> GetAsync(Guid id)
		{
			var product = await productRepository.GetAsync(id);
			return ObjectMapper.Map<Product, ProductDto>(product);
		}

		public async Task UpdateAsync(Guid id, CreateUpdateProductDto input)
		{
			var product = await productRepository.GetAsync(id);
			ObjectMapper.Map(input, product);
		}

		public async Task DeleteAsync(Guid id)
		{
			await productRepository.DeleteAsync(id);
		}

		[UnitOfWork(isTransactional: true)]
		public async Task TodoSomethingAsync()
		{
			using (var uow = unitOfWorkManager.Begin(
				requiresNew: true,
				isTransactional: true,
				timeout: 2000))
			{
				await productRepository.InsertAsync(new Product() { });
				await productRepository.InsertAsync(new Product() { });
				await uow.CompleteAsync();
			}
		}
		*/
	}
}
