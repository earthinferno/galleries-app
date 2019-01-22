using GalleriesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GalleriesServer.Services
{
    public class OwnerService
    {
        private readonly GalleriesDbContext _dbContext;
        public OwnerService(GalleriesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal void AddOwner(string emailAddress, string identityProvider, string userId, string firstName, string lastName)
        {
            Owner owner = new Owner()
            {
                EmailAddress = emailAddress,
                ExternalIdentityProvider = identityProvider,
                ExternalUserId = userId,
                FirstName = firstName,
                LastName = lastName
            };
            _dbContext.Owners.Add(owner);
            _dbContext.SaveChanges();

        }

        internal async Task<Owner> GetOwner(string userId)
        {
            var owner = await _dbContext.Owners.Where(a => a.ExternalUserId == userId).ToListAsync();
            if (owner == null || owner.Count() == 0)
            {
                return null;
            }

            //Just in case of duplicates
            //TODO: in case of duplicates is a problem: THERE MUST NOT BE DUPLICATES!
            return owner.First();
        }

        internal async Task AddOwner(Owner owner)
        {
            _dbContext.Owners.Add(owner);
            await _dbContext.SaveChangesAsync();
        }

        // Not used but may be useful
        internal async Task<Owner> GetOwnerOrNewIfNotExists(string userId)
        {
            var owner = await GetOwner(userId);
            if (owner == null)
            {
                AddOwner("Undefined", "Auth0", userId, "Undefined", "Undefined");
                owner = await GetOwner(userId);
            }

            return owner;
        }


    }
}
