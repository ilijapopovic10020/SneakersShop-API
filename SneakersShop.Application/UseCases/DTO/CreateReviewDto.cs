using System;

namespace SneakersShop.Application.UseCases.DTO;

public class CreateReviewDto
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}
