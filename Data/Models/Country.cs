using System;
using System.Collections.Generic;

namespace HandleHjelp.Data.Models;

public partial class Country
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Longitude { get; set; }

    public decimal Latitude { get; set; }
}
