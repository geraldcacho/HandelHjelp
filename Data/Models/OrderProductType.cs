using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class OrderProductType
{
    public string OrderId { get; set; } = null!;

    public int ProductTypeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ProductType ProductType { get; set; } = null!;
}
