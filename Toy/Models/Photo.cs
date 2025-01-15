using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Photo
{
    public int Id { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public bool? IsMain { get; set; }

    public virtual ICollection<PhotoBrand> PhotoBrands { get; set; } = new List<PhotoBrand>();

    public virtual ICollection<PhotoProductOnOrder> PhotoProductOnOrders { get; set; } = new List<PhotoProductOnOrder>();

    public virtual ICollection<PhotoProduct> PhotoProducts { get; set; } = new List<PhotoProduct>();

    public virtual ICollection<PhotoReview> PhotoReviews { get; set; } = new List<PhotoReview>();
}
