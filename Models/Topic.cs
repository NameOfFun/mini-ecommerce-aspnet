using System;
using System.Collections.Generic;

namespace Mini_E_Commerce.Models;

public partial class Topic
{
    public int TopicId { get; set; }

    public string? TopicName { get; set; }

    public string? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
