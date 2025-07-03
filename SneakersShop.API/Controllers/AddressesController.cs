using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Addresses;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Addresses;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController(ICommandHandler commandHandler, IQueryHandler queryHandler) : ControllerBase
    {
        private readonly ICommandHandler _commandHandler = commandHandler;
        private readonly IQueryHandler _queryHandler = queryHandler;

        [HttpGet]
        public IActionResult Get([FromQuery] BaseSearch search, [FromServices] IGetAddressesQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateAddressDto dto, [FromServices] ICreateAddressCommand command)
        {
            _commandHandler.HandleCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateAddressDto dto, [FromServices] IUpdateAddressCommand command)
        {
            dto.Id = id;

            _commandHandler.HandleCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteAddressCommand command)
        {
            _commandHandler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
