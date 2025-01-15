using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PurchaseHistoryProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int PurchaseHistoryId { get; set; }

    public decimal Price { get; set; }

    public int Amount { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual PurchaseHistory PurchaseHistory { get; set; } = null!;
}
