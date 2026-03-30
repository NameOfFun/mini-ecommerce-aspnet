using System;
using System.Collections.Generic;

namespace Mini_E_Commerce.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? SelectedDate { get; set; }

    public string? Description { get; set; }

    public string? UserId { get; set; }

    public virtual AppUser? User { get; set; }
    
    public virtual Product? Product { get; set; }
}
