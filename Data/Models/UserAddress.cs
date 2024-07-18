using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class UserAddress
{
    public int UserAddressId { get; set; }

    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Longitude { get; set; }

    public decimal Latitude { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Postcode { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }
}
