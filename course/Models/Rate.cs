using System;
using System.Collections.Generic;

namespace course.Models;

public partial class Rate
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
