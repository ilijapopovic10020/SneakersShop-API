using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.API.Extensions;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.Commands.Brands;
using SneakersShop.Application.UseCases.Queries.Brands;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using Microsoft.AspNetCore.Authorization;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BrandsController(ICommandHandler commandHandler, IQueryHandler queryHandler) : ControllerBase
    {
        private readonly ICommandHandler _commandHandler = commandHandler;
        private readonly IQueryHandler _queryHandler = queryHandler;

        // GET: api/<BrandsController>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] BaseSearch search, [FromServices] IGetBrandsQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }

        // GET: api/<BrandsController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id, [FromServices] IFindBrandQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, id));
        }

        // POST: api/<BrandsController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateBrandDto dto, [FromServices] ICreateBrandCommand command)
        {
            _commandHandler.HandleCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/<BrandsController>/5
        // [HttpPut("{id}")]
        // public IActionResult Put(int id, [FromBody] BrandDto dto)
        // {
        //     return NoContent();
        // }

        // // DELETE: api/<BrandsController>/5
        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        //     return NoContent();
        // }
    }
}
