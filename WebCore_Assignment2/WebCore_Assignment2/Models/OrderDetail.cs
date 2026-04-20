using System;
using System.Collections.Generic;

namespace WebCore_Assignment2.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    public double? UnitAmount { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Order? Order { get; set; }
}
