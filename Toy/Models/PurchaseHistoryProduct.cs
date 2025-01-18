using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PurchaseHistoryProduct
{
    public int Id { get; set; }

    public int PurchaseHistoryId { get; set; }
    public int PurchaceId { get; set; }

    public virtual PurchaseHistory PurchaseHistory { get; set; } = null!;
}
