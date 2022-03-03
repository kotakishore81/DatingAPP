using Dating_API.Data;
using Dating_API.DTOs;
using Dating_API.Entites;
using Dating_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dating_API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _token;
        public AccountController(DataContext context, ITokenService token)
        {
            _context = context;
            _token = token;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTOs>> Regiser(Register register)
        {
            if (await UserExists(register.userName)) { return BadRequest("User Already Exist"); }
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = register.userName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDTOs {
                username = user.UserName,
                token = _token.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTOs>> Login(LoginDTOs login) {
            var user =  await _context.Users.SingleOrDefaultAsync(x => x.UserName == login.userName);
            if (user == null) return Unauthorized("Invalid User");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.password));
            for (int i = 0; i < computerHash.Length; i++) {
                if (computerHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTOs
            {
                username = user.UserName,
                token = _token.CreateToken(user)
            };
        }
    }
}
