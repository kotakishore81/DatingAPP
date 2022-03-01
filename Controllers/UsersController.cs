using Dating_API.Data;
using Dating_API.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context) {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>>GetUsers() {
            return await _context.Users.ToListAsync();
          
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int Id)
        {
            return await _context.Users.FindAsync(Id);
            
        }

    }
}
