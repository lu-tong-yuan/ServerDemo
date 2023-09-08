using Microsoft.IdentityModel.Tokens;
using serverDemo.Dtos;
using serverDemo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace serverDemo.Services
{
    public class UserService
    {
        private readonly ServerDBContext _dbContext;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public UserService(ServerDBContext dbContext, PasswordService passwordService, IConfiguration configuration)
        { 
            _dbContext = dbContext;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public User GetUser(Guid id)
        { 
            var result = (from a in _dbContext.User
                          where a.UserID == id
                          select a).SingleOrDefault();
            if (result != null) 
            {
                return result;
            }
            return null;
        }

        public Boolean GetUserByName(string name)
        {
            var result = (from a in _dbContext.User
                          where a.Name == name
                          select a).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public User GetUserByEmail(string email)
        {
            var result = (from a in _dbContext.User
                          where a.Email == email
                          select a).SingleOrDefault();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public Boolean CheckPassword(string password, string hashPassword)
        {
            if (_passwordService.VerifyPassword(password, hashPassword)) 
            {
                return true;
            }
            return false;
        }

        public User Register(UserDto value) 
        {
            value.Password = _passwordService.HashPassword(value.Password);
            User insert = new User();
            _dbContext.User.Add(insert).CurrentValues.SetValues(value);
            _dbContext.SaveChanges();
            return insert;
        }

        public string Login(User value)
        {
            var token = GenerateJwtToken(value);
            return token; 
        }

        private string GenerateJwtToken(User value)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, value.Name),
                new Claim(ClaimTypes.NameIdentifier, value.UserID.ToString()),
                new Claim(ClaimTypes.Role, value.Role),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
