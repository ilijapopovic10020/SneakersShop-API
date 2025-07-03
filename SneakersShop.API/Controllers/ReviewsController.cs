using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Reviews;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Reviews;
using SneakersShop.Implementation.Handling;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController(IQueryHandler queryHandler, ICommandHandler commandHandler) : ControllerBase
    {
        private readonly IQueryHandler _queryHandler = queryHandler;
        private readonly ICommandHandler _commandHandler = commandHandler;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] PagedSearchId search, [FromServices] IGetReviewsQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReviewDto dto, [FromServices] ICreateReviewCommand command)
        {
             _commandHandler.HandleCommand(command, dto);
             return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateReviewDto dto, [FromServices] IUpdateReviewCommand command)
        {
            dto.Id = id;
            _commandHandler.HandleCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteReviewCommand command)
        {
            _commandHandler.HandleCommand(command, id);
            return NoContent();
        }

    }
}
