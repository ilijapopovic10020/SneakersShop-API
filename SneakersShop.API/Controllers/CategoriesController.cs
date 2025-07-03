using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Application.Handling;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Categories;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController(IQueryHandler queryHandler) : ControllerBase
    {

        private readonly IQueryHandler _queryHandler = queryHandler;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] BaseSearch search, IGetCategoriesQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, search));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, IFindCategoryQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, id));
        }
    }
}
