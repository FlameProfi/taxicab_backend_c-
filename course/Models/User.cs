using System;
using System.Collections.Generic;

namespace course.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string Password { get; set; } = null!;

    public string? Number { get; set; }

    public string? Sex { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int AllOrders { get; set; }

    public int GoodFeedBack { get; set; }

    public int Income { get; set; }

    public string RankId { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual Rank Rank { get; set; } = null!;
}
