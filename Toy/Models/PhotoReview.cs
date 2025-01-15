using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PhotoReview
{
    public int Id { get; set; }

    public int ReviewId { get; set; }

    public int PhotoId { get; set; }

    public virtual Photo Photo { get; set; } = null!;

    public virtual Review Review { get; set; } = null!;
}
