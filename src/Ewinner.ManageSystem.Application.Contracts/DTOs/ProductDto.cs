using Ewinner.ManageSystem.Consts;
using System;
using Volo.Abp.Application.Dtos;

namespace Ewinner.ManageSystem.DTOs
{
	public class ProductDto : AuditedEntityDto<Guid>
	{
		public string CategoryName { get; set; }
		public Guid CategoryId { get; set; }
		public string Name { get; set; }
		public float Price { get; set; }
		public bool IsFreeCargo { get; set; }
		public DateTime ReleaseDate { get; set; }
		public ProductStockState StockState { get; set; }
	}
}
