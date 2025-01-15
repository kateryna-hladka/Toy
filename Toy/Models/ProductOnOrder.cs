using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class ProductOnOrder
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<PhotoProductOnOrder> PhotoProductOnOrders { get; set; } = new List<PhotoProductOnOrder>();

    public virtual User? User { get; set; }
}
