using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PurchaseHistory
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Price { get; set; }

    public short Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<PurchaseHistoryProduct> PurchaseHistoryProducts { get; set; } = new List<PurchaseHistoryProduct>();

    public virtual User User { get; set; } = null!;
}
