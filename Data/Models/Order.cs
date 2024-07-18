using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public string CustomerId { get; set; } = null!;

    public byte[]? Image { get; set; }

    public string? City { get; set; }

    public decimal TotalAmount { get; set; }

    public string? StreetAddressLine1 { get; set; }

    public string? PostalCode { get; set; }

    public string? CountryName { get; set; }

    public string? CountryCode { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? ReceivedOn { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<OrderProductType> OrderProductTypes { get; set; } = new List<OrderProductType>();

    public virtual ICollection<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();
}
