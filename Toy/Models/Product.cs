using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Product
{
    public int Id { get; set; }

    public byte AgeFrom { get; set; }

    public byte? AgeTo { get; set; }

    public int? CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int? BrandId { get; set; }

    public string? Description { get; set; }

    public short Amount { get; set; }

    public int? CountryProducerId { get; set; }

    public int? MaterialId { get; set; }

    public int? PackagingId { get; set; }

    public string? Sex { get; set; }

    public decimal? Weight { get; set; }

    public int? WeightUnitId { get; set; }

    public decimal? Size { get; set; }

    public int? SizeUnitId { get; set; }

    public int? PriceUnitId { get; set; }

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual CountryProducer? CountryProducer { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Packaging? Packaging { get; set; }

    public virtual ICollection<PhotoProduct> PhotoProducts { get; set; } = new List<PhotoProduct>();

    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

    public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; } = new List<PurchaseHistory>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Unit? SizeUnit { get; set; }

    public virtual Unit? WeightUnit { get; set; }
    public virtual Unit? PriceUnit { get; set; }
}
