using Microsoft.AspNetCore.Mvc;
using RestaurationAPI.Models;
using RestaurationAPI.Service;

namespace RestaurationAPI.Controllers
{

    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int restaurantId)
        {
            _dishService.RemoveAll(restaurantId);

            return NoContent();

        }

        [HttpDelete("{dishId}")]
        public ActionResult Delete([FromRoute]int restaurantId, [FromRoute] int dishId)
        {
            _dishService.Remove(restaurantId, dishId);

            return NoContent();

        }


        [HttpPost]
        public ActionResult Post([FromRoute]int restaurantId, CreateDishDto dto)
        {
           var dishId =  _dishService.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{dishId}",null);
        }
        [HttpGet("{dishId}")]
        public ActionResult <DishDto> Get([FromRoute] int restaurantId, [FromRoute]int dishId)
        {
            DishDto dish = _dishService.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
        {
            var dishes = _dishService.GetAll(restaurantId);

            return Ok(dishes);
        }



    }
}
