using System;
using System.Collections.Generic;

namespace course.Models;

public partial class Car
{
    public string Id { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Number { get; set; } = null!;

    public int Year { get; set; }

    public string Color { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
