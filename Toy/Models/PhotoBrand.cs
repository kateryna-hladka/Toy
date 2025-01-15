using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PhotoBrand
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public int PhotoId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Photo Photo { get; set; } = null!;
}
