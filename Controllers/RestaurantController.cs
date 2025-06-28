using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurationAPI.Entities;
using RestaurationAPI.Models;
using RestaurationAPI.Service;
using System.Security.Claims;

namespace RestaurationAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController] // powoduje to automatyczne wywołanie tego ;
    [Authorize] // dostęp jesli ktos ma tokoen Jwt

    /*
       if (!ModelState.IsValid) return BadRequest();
     */

    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{idx}")]
        public ActionResult Modify([FromBody]ModifyRestaurantDto dto, [FromRoute]int idx)
        {

            _restaurantService.Modify(idx, dto);

            return Ok();
        }
        [HttpDelete("{idx}")]
        public ActionResult Delete([FromRoute]int idx)
        {
            _restaurantService.Delete( idx);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] // Tylko Manager ma dostęp
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Wypisanie roli w konsoli
            Console.WriteLine($"User's role: {userRole}");


            var id = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{id}",null);
        }


        [HttpGet]
        // [Authorize(Policy = "AtleastCreatedTwoRestaurant")]
        [AllowAnonymous]
       public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query)
        {


            var restaurantDtos = _restaurantService.GetAll(query);


            return Ok(restaurantDtos);
        }

        [HttpGet("{idx}")]
        [AllowAnonymous] // wyjątek zapytania bez nagłówka autoryzacji
        public ActionResult<RestaurantDto> GetOne([FromRoute]int idx)
        {
            var restaurant = _restaurantService.GetById(idx);

            return Ok(restaurant);
        }

        public ActionResult Createsome()
        {
            return Ok();
        }
    }
}
