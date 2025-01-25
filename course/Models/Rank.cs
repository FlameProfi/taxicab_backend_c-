using System;
using System.Collections.Generic;

namespace course.Models;

public partial class Rank
{
    public string Id { get; set; } = null!;

    public string Position { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
