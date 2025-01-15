using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PhotoProductOnOrder
{
    public int Id { get; set; }

    public int ProductOnOrderId { get; set; }

    public int PhotoId { get; set; }

    public virtual Photo Photo { get; set; } = null!;

    public virtual ProductOnOrder ProductOnOrder { get; set; } = null!;
}
