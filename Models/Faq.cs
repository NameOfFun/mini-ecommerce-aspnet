using System;
using System.Collections.Generic;

namespace Mini_E_Commerce.Models;

public partial class Faq
{
    public int OrderId { get; set; }

    public string Question { get; set; } = null!;

    public string Reply { get; set; } = null!;

    public DateOnly PostedDate { get; set; }

    public string EmployeeId { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
