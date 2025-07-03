using System;

namespace SneakersShop.Application.UseCases.DTO;

public class ReviewsDto : BaseDto
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string UserImage { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public string CreatedAt { get; set; }
}
