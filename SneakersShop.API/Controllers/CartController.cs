using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Carts;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Carts;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController(ICommandHandler commandHandler, IQueryHandler queryHandler) : ControllerBase
    {
        private readonly ICommandHandler _commandHandler = commandHandler;
        private readonly IQueryHandler _queryHandler = queryHandler;

        [HttpGet]
        public IActionResult Get([FromServices] IFindCartQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query));
        }

        [HttpPost]
        public IActionResult Post([FromBody] List<CartDto> cartItems, [FromServices] IUpsertCartCommand command)
        {
            _commandHandler.HandleCommand(command, cartItems);
            return Ok();
        }
    }
}
