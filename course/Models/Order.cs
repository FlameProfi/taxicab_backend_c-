using System;
using System.Collections.Generic;

namespace course.Models;

public partial class Order
{
    public string Id { get; set; } = null!;

    public string StartingPoint { get; set; } = null!;

    public string EndPoint { get; set; } = null!;

    public int Price { get; set; }

    public string? Driver { get; set; }

    public int? Tips { get; set; }

    public string RateId { get; set; } = null!;

    public virtual Rate Rate { get; set; } = null!;
}
