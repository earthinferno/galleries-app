using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GalleriesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly GalleriesDbContext _dbContext;
        private readonly OwnerService _ownerService;


        public OwnerController(GalleriesDbContext dbContext, OwnerService ownerService)
        {
            _dbContext = dbContext;
            _ownerService = ownerService;

        }

        /// <summary>
        /// Returns the owner specified by given UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: api/Owner/userId
        [HttpGet("{userId}")]
        public async Task<ActionResult<Owner>> GetOwner(string userId)
        {
            var owner = await _dbContext.Owners.Where(a => a.ExternalUserId == userId).ToListAsync();
            if (owner == null || owner.Count() == 0)
            {
                return NotFound();
            }

            //Just in case of duplicates
            //TODO: in case of duplicates is a problem: THERE MUST NOT BE DUPLICATES!
            return owner.First();
        }

        /// <summary>
        /// Creates a new instance of owner.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        // POST: api/Owner
        [HttpPost]
        public async Task<ActionResult<Owner>> PostOwner(Owner owner)
        {
            _dbContext.Owners.Add(owner);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("GetOwner", new { userId = owner.ExternalUserId }, owner);
        }

        /// <summary>
        /// Updates the owner record to new supplied values for specified owner id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        // PUT: api/Owner/5
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutOwner(string userId, Owner owner)
        {
            if (userId != owner.ExternalUserId)
            {
                return BadRequest();
            }

            _dbContext.Entry(owner).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Owner/5
        [HttpDelete("{userId}")]
        public async Task<ActionResult<IEnumerable<Owner>>> DeleteOwner(string userId)
        {
            //var gallery = await _dbContext.MediaContainers.FindAsync(userId);
            var owners = await _dbContext.Owners.Where(a => a.ExternalUserId == userId).ToListAsync();
            if (owners == null || owners.Count() == 0)
            {
                return NotFound();
            }

            foreach (var owner in owners)
            {
                _dbContext.Owners.Remove(owner);
                await _dbContext.SaveChangesAsync();
            }
            return owners;
        }
    }
}

/*
 * 
 * 
 
 */