using System;
using System.Collections.Generic;

namespace Mini_E_Commerce.Models;

public partial class Feedback
{
    public string FeedbackId { get; set; } = null!;

    public int TopicId { get; set; }

    public string Content { get; set; } = null!;

    public DateOnly FeedbackDate { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool NeedsReply { get; set; }

    public string? Reply { get; set; }

    public DateOnly? ReplyDate { get; set; }

    public virtual Topic Topic { get; set; } = null!;
}
