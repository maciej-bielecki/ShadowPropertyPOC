using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaceloRex.WebApplication.Services
{
    public class UserContextService : IUserContextService
    {
        public Guid GetUserId()
        {
            return Guid.NewGuid();
        }

    }
}
