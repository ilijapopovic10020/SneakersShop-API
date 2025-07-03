using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Products;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Products;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController(IQueryHandler queryHandler, ICommandHandler commandHandler) : ControllerBase
    {
        private readonly IQueryHandler _queryHandler = queryHandler;
        private readonly ICommandHandler _commandHandler = commandHandler;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] ProductsSearch search, [FromServices] IGetProductsQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id, [FromServices] IFindProductQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateProductDto dto, [FromServices] ICreateProductCommand command)
        {
            _commandHandler.HandleCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
