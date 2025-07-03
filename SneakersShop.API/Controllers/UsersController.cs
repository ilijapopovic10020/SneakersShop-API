using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.API.Extensions;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Users;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Users;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(ICommandHandler commandHandler, IQueryHandler queryHandler) : ControllerBase
    {
        private readonly ICommandHandler _commandHandler = commandHandler;
        private readonly IQueryHandler _queryHandler = queryHandler;

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IFindUserQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, id));
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] CreateUserDto dto, [FromServices] ICreateUserCommand command)
        {
            _commandHandler.HandleCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserDto dto, [FromServices] IUpdateUserCommand command)
        {
            dto.Id = id;
            _commandHandler.HandleCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserCommand command)
        {
            _commandHandler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
