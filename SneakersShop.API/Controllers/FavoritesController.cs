using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Favorites;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries;
using SneakersShop.Application.UseCases.Queries.Favorites;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController(ICommandHandler commandHandler, IQueryHandler queryHandler) : ControllerBase
    {
        private readonly ICommandHandler _commandHandler = commandHandler;
        private readonly IQueryHandler _queryHandler = queryHandler;

        [HttpGet]
        public IActionResult Get([FromQuery] FavoriteSearch search, [FromServices] IGetFavoritesQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateFavoriteDto dto, [FromServices] ICreateFavoriteCommand command)
        {
            _commandHandler.HandleCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteFavoriteCommand command)
        {
            _commandHandler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
