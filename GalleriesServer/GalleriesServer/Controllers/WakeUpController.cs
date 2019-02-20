using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GalleriesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WakeUpController
    {

        private readonly GalleriesDbContext _dbContext;
        private readonly OwnerService _ownerService;


        public WakeUpController(GalleriesDbContext dbContext, OwnerService ownerService)
        {
            _dbContext = dbContext;
            _ownerService = ownerService;

        }

        [HttpGet]
        public async Task<ActionResult<int>> GetAccount()
        {
            return await _dbContext.Owners.CountAsync();
        }
    }
}
