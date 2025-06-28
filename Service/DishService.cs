using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurationAPI.Entities;
using RestaurationAPI.Exceptions;
using RestaurationAPI.Models;

namespace RestaurationAPI.Service
{
 public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int resturantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
        void Remove(int restaurantId, int dishId);
    }
    public class DishService : IDishService
    {
        private readonly RestaurantDBContext _context;
        private readonly IMapper _mapper;
        public DishService(RestaurantDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestauranById(restaurantId);

            _context.RemoveRange(restaurant.Dishes);
            _context.SaveChanges();


        }

        public void Remove(int restaurantId, int dishId)
        {

            var dish = GetDishById(restaurantId, dishId);
            _context.Dishes.Remove(dish);
            _context.SaveChanges();

        }
        public int Create(int restaurantId,[FromBody] CreateDishDto dto)
        {
            var restaurant = GetRestauranById(restaurantId);


            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;
            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestauranById(restaurantId);


            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);

            if(dish == null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestauranById(restaurantId);

            if (restaurant == null) throw new NotFoundException("Restaurant not found");


            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishDtos;
        }

        private Restaurant GetRestauranById(int restaurantId)
        {
            var restaurant = _context
             .Restaurants
             .Include(r => r.Dishes)
             .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null) throw new NotFoundException("Restaurant not found");

            return restaurant;

        }

        private Dish GetDishById(int restaurantId, int dishId)
        {
            var dish = _context
                .Dishes
                .FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId);
            if (dish == null) throw new NotFoundException("Dish not found");

            return dish;


        }
    }
}
