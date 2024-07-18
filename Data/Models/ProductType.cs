using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<OrderProductType> OrderProductTypes { get; set; } = new List<OrderProductType>();
}
