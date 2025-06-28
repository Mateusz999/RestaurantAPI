using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurationAPI.Authorization;
using RestaurationAPI.Entities;
using RestaurationAPI.Exceptions;
using RestaurationAPI.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace RestaurationAPI.Service
{
    public class RestaurantService : IRestaurantService
    {

        private readonly RestaurantDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public IAuthorizationService _authorizationService;

        public IUserContextService _userContextService { get; }

        public RestaurantService(RestaurantDBContext context, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }


        public void Modify(int idx, ModifyRestaurantDto dto)
        {

            var restaurant = _context
              .Restaurants
              .FirstOrDefault(r => r.Id == idx);

            if (restaurant == null) throw new NotFoundException("Restauration not found");


            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, 
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;


            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }
            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;


            _context.SaveChanges();

        }
        

        public void Delete(int idx)
        {

            _logger.LogError($"Restaurant with id: {idx} DELETE action invoked");
            var restaurant = _context
                .Restaurants
                .FirstOrDefault(r => r.Id == idx);

            if (restaurant == null) throw new NotFoundException("Restauration not found");


            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
          new ResourceOperationRequirement(ResourceOperation.Delete)).Result;


            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }


            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();

        }

        public RestaurantDto GetById(int idx)
        {
            var restaurant = _context
       .Restaurants
       .Include(r => r.Dishes) // to są po prostu joiny
       .Include(r => r.Address)
       .FirstOrDefault(r => r.Id == idx);



            if (restaurant == null)
            {
                throw new NotFoundException("Restauration not found");
            }
            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }


        public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
        {


            var baseQuery = _context.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhraze == null || r.Name.ToLower().Contains(query.SearchPhraze.ToLower()) ||
                            r.Description.ToLower().Contains(query.SearchPhraze.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {

                var columnSelector = new Dictionary<string, Expression<Func<Restaurant, Object>>>{
                    { nameof(Restaurant.Name), r=> r.Name },
                    { nameof(Restaurant.Description), r=> r.Description },
                    { nameof(Restaurant.Category), r=> r.Category }
                };

                var selectedColumn = columnSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC?
                    baseQuery.OrderBy(selectedColumn)
                    :baseQuery.OrderByDescending(selectedColumn);
            }

            var resturants =  baseQuery
                .Skip(query.pageSize * (query.pageNumber - 1 ))
                .Take(query.pageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();


            var restaurantDtos = _mapper.Map<List<RestaurantDto>>(resturants);
            var result = new PagedResult<RestaurantDto>(restaurantDtos, totalItemsCount, query.pageSize,query.pageNumber);
            return result;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
            return restaurant.Id;

        }


    }
}
