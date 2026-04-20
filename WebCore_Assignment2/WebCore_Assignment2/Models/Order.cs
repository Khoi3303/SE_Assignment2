using System;
using System.Collections.Generic;

namespace WebCore_Assignment2.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? AgentId { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
