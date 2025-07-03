using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Password;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController(ICommandHandler commandHandler) : ControllerBase
    {
        private ICommandHandler _commandHandler = commandHandler;

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdatePasswordDto dto, [FromServices] IUpdatePasswordCommand command)
        {
            dto.Id = id;
            _commandHandler.HandleCommand(command, dto);
            return NoContent();
        }
    }
}
