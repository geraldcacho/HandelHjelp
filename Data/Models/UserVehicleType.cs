using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class UserVehicleType
{
    public string UserId { get; set; } = null!;

    public int VehicleTypeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }
}
