using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Extensions;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GalleriesServer.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly GalleriesDbContext _dbContext;
        private readonly OwnerService _ownerService;


        public RegistrationController(GalleriesDbContext dbContext, OwnerService ownerService)
        {
            _dbContext = dbContext;
            _ownerService = ownerService;

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
            try
            {
                Owner createdOwner = await _ownerService.AddOwner(owner);
                //return CreatedAtAction(actionName: nameof(AccountsController.GetAccount), controllerName: nameof(AccountsController), routeValues: new { userId = owner.ExternalUserId }, value: createdOwner);
                return Created("/api/Accounts", createdOwner);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}
