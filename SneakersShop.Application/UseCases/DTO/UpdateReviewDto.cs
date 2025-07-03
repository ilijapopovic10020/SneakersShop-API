using System;

namespace SneakersShop.Application.UseCases.DTO;

public class UpdateReviewDto : UpdateEntityDto
{
    public string Comment { get; set; }
    public int Rating { get; set; }
}
