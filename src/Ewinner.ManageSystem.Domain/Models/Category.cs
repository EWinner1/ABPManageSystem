using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ewinner.ManageSystem.Models
{
	public class Category : AuditedAggregateRoot<Guid>
	{
		public string Name { get; set; }
	}
}
