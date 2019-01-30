using System;
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
    public class AccountsController : Controller
    {
        private readonly GalleriesDbContext _dbContext;
        private readonly OwnerService _ownerService;


        public AccountsController(GalleriesDbContext dbContext, OwnerService ownerService)
        {
            _dbContext = dbContext;
            _ownerService = ownerService;

        }


        /// <summary>
        /// Returns the account specified by given UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: api/Account/id
        [HttpGet("{userId}")]
        public async Task<ActionResult<Owner>> GetAccount(string userId)
        {
            var owner = await _ownerService.GetOwner(userId);
            //if (owner == null)
            //{
            //    return NotFound();
            //}
            return owner;
        }


        /// <summary>
        /// Updates the account record to new supplied values for specified owner id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        // PUT: api/Account/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAccount(string userId, Owner owner)
        {
            if (userId != owner.ExternalUserId)
            {
                return BadRequest();
            }

            try
            {
                await _ownerService.UpdateOwner(owner);
            }
            catch (Exception e)
            {
                return Conflict(e);
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // DELETE: api/Account/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            try
            {
                await _ownerService.DeleteOwner(id);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
            return NoContent();
        }
    }
}

/*
 * 
 * 
 
 */
