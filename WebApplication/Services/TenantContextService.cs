using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaceloRex.WebApplication.Services
{
    public class TenantContextService : ITenantContextService
    {
        public Guid GetTenantId()
        {
            return new Guid("14F7D3D2-922B-448C-AAFA-172FF4EFC2D3");
        }
    }
}
