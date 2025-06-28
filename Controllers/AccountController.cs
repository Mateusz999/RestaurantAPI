using Microsoft.AspNetCore.Mvc;
using RestaurationAPI.Models;
using RestaurationAPI.Service;

namespace RestaurationAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody]RegisterUserDto dto)
        {
            _service.RegisterUser(dto);

            return Ok();
        }


        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _service.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
/*
 
 {
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "1990-05-15T00:00:00",
  "nationality": "Polish",
  "password": "123123",
  "confirmPassword": "123123",
  "roleId": 2
}


Beaers eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMzIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IiAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJNYW5hZ2VyICIsIkRhdGhPZkJpcnRoIjoiMTk5MC0wMC0xNSIsIk5hdGlvbmFsaXR5IjoiUG9saXNoIiwiZXhwIjoxNzUyMjQ3NjkyLCJpc3MiOiJodHRwOi8vcmVzdGF1cmFudGFwaS5jb20iLCJhdWQiOiJodHRwOi8vcmVzdGF1cmFudGFwaS5jb20ifQ.Al6Xh6HIpIfmCBNEYCkWBarXlvdnMTiWfxnK-xcBoQ8
 */