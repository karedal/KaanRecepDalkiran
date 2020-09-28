using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sum.Api.Data;
using Sum.Api.Helper;
using Sum.Api.Models;

namespace Sum.Api.Service
{
    public class UserService : IUserService
    {
        protected readonly SumDbContext _dbContext;
        public readonly JwtSettings _jwtSettings;
        public UserService(JwtSettings jwtSettings, SumDbContext dbContext)
        {
            _jwtSettings = jwtSettings;
            _dbContext = dbContext;
        }


        public async Task<AuthenticationResult> RegisterAsync(RegisterDto model)
        {
            var existUser = await _dbContext.Users.Where(c=>c.Email.Equals(model.Email)).ToListAsync();

            if (existUser.Any())
            {
                return new AuthenticationResult
                {
                    ReadableMessage = "This email address already exists",
                    Success = false
                };
            }

            User user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = model.Email.Trim(),
                PasswordHash = Help.Hashing(model.PasswordHash.Trim())
            };

            var createdUser = _dbContext.Users.Add(user);
            
            return new AuthenticationResult
            {
                ReadableMessage = "The Process is success",
                Success = true,
                Token = GenerateToken(createdUser.Entity)
            };
        }

        public async Task<AuthenticationResult> LoginAsync(LoginDto model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(c => c.Email.Equals(model.Email) && c.PasswordHash == Help.Hashing(model.Password.Trim()));

            if (user == null)
            {
                return new AuthenticationResult
                {
                    ReadableMessage = "User/Password is wrong",
                    Success = false
                };
            }
            return new AuthenticationResult
            {
                ReadableMessage = "The Process is success",
                Success = true,
                Token = GenerateToken(user)
            };
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.FirstName + " " + user.LastName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                    new Claim("id",user.Id.ToString()),

                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

      
        
    }
}