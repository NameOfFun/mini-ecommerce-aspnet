using System;
using System.Collections.Generic;

namespace Mini_E_Commerce.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string CustomerId { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public string? FullName { get; set; }

    public string Address { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string ShippingMethod { get; set; } = null!;

    public double ShippingFee { get; set; }

    public int StatusId { get; set; }

    public string? EmployeeId { get; set; }

    public string? Note { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus Status { get; set; } = null!;
}
