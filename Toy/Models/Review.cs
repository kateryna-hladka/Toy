using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? Comment { get; set; }

    public byte Mark { get; set; }

    public string? Advantages { get; set; }

    public string? Disadvantages { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<PhotoReview> PhotoReviews { get; set; } = new List<PhotoReview>();

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
