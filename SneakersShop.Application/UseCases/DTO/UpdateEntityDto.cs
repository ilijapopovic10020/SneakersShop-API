using System;
using System.Text.Json.Serialization;

namespace SneakersShop.Application.UseCases.DTO;

public class UpdateEntityDto
{
    [JsonIgnore]
    public int Id { get; set; }
}
