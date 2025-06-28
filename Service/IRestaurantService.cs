using RestaurationAPI.Models;
using System.Security.Claims;

namespace RestaurationAPI.Service
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        RestaurantDto GetById(int idx);

        void Delete(int idx);

        void Modify(int idx, ModifyRestaurantDto dto);
    }
}// test
