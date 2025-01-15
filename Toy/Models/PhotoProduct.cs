using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PhotoProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int PhotoId { get; set; }

    public virtual Photo Photo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
