using RestaurationAPI.Models;
using System.Security.Claims;

namespace RestaurationAPI.Service
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int idx);

        void Delete(int idx);

        void Modify(int idx, ModifyRestaurantDto dto);
    }
}