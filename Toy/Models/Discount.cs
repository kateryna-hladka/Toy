using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Discount
{
    public int Id { get; set; }

    public decimal Value { get; set; }

    public int UnitId { get; set; }

    public DateTime DateTimeStart { get; set; }

    public DateTime DateTimeEnd { get; set; }

    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

    public virtual Unit Unit { get; set; } = null!;
}
