using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class OrderStatus
{
    public string OrderId { get; set; } = null!;

    public int StatusId { get; set; }

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
