using Microsoft.AspNetCore.Mvc;
using serverDemo.Dtos;
using serverDemo.Services;
using serverDemo.Validators;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace serverDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService) 
        {
            _userService = userService;
        }

        //// GET: api/<UserController>
        //[HttpGet("testAPI")]
        //public string Get()
        //{
        //    return "成功連結user route...";
        //}

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            var result = _userService.GetUser(id);
            if (result == null) 
            {
                return NotFound("找不到使用者");
            }
            return Ok(result);
        }

        // POST api/<UserController>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto value)
        {
            var validator = new RegisterValidator();
            var result = validator.Validate(value);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            if (_userService.GetUserByName(value.Name)) 
            {
                return BadRequest("此信箱已經被註冊過了");
            }

            var insert = _userService.Register(value);
            return Ok(new { msg = "使用者成功儲存。", User = insert });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto value)
        {
            var validator = new LoginValidator();
            var result = validator.Validate(value);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var foundUser = _userService.GetUserByEmail(value.Email);
            if (foundUser == null)
            {
                return BadRequest("無法找到使用者。請確認信箱是否正確。");
            }

            if (!_userService.CheckPassword(value.Password, foundUser.Password)) 
            {
                return Unauthorized("密碼錯誤");
            }

            var token = _userService.Login(foundUser);

            return Ok(new { msg = "成功登入", Token = "Bearer " + token, User = foundUser });

        }
    }
}
