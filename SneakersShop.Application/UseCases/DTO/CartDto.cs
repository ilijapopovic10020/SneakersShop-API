using System;
using System.Drawing;

namespace SneakersShop.Application.UseCases.DTO;

public class CartDto
{
    public ProductDto Product { get; set; } = new();
    public SizeDto Size { get; set; } = new();
    public int Quantity { get; set; }
    public bool IsSelected { get; set; }
}
