using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Cities;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitiesController(ICommandHandler commandHandler, IQueryHandler queryHandler) : ControllerBase
    {
        private readonly ICommandHandler _commandHandler = commandHandler;
        private readonly IQueryHandler _queryHandler = queryHandler;

        // GET: api/<BrandsController>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] BaseSearch search, [FromServices] IGetCitiesQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }
    }
}
