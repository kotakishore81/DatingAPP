using Dating_API.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_API.Interfaces
{
   public interface ITokenService
    {
       public string CreateToken(AppUser user);

    }
}
