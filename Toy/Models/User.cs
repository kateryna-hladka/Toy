using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual ICollection<ProductOnOrder> ProductOnOrders { get; set; } = new List<ProductOnOrder>();

    public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; } = new List<PurchaseHistory>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
