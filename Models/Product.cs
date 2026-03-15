using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mini_E_Commerce.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductAlias { get; set; }

    public int CategoryId { get; set; }

    public string? UnitDescription { get; set; }

    public double? UnitPrice { get; set; }

    public string? Image { get; set; }

    public DateTime ManufactureDate { get; set; }

    public double Discount { get; set; }

    public int ViewCount { get; set; }

    public string? Description { get; set; }

    public string SupplierId { get; set; } = null!;
    [JsonIgnore]
    public virtual Category Category { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    [JsonIgnore]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    [JsonIgnore]
    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();

    public virtual Supplier Supplier { get; set; } = null!;
}
