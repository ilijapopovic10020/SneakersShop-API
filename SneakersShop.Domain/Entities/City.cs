using System;

namespace SneakersShop.Domain.Entities;

public class City : Entity
{
    public string Name { get; set; }
    public string ZipCode { get; set; }
}
