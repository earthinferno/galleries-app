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
            var createdOwner = _dbContext.Owners.Add(owner);
            _dbContext.SaveChanges();
        }

        internal async Task<Owner> GetOwner(string userId)
        {
            var owner = await _dbContext.Owners.Where(a => a.ExternalUserId == userId).ToListAsync();
            if (owner == null || owner.Count() == 0)
            {
                return null;
            }

            return owner.First();
        }

        internal async Task<Owner> AddOwner(Owner owner)
        {

            //var result = _dbContext.Owners.Where(a => a.ExternalUserId == owner.ExternalUserId || a.EmailAddress == owner.EmailAddress).Count()

            if (_dbContext.Owners.Where(a => a.ExternalUserId == owner.ExternalUserId).Count() > 0)
            {
                throw new RepositoryException(RepositiryExceptionType.DuplicateUser, owner.ExternalUserId);
            }
            if (_dbContext.Owners.Where(a => a.EmailAddress == owner.EmailAddress).Count() > 0)
            {
                throw new RepositoryException(RepositiryExceptionType.DuplicateEmailAddress, owner.EmailAddress);
            }
            var createrOwner = _dbContext.Owners.Add(owner);
            await _dbContext.SaveChangesAsync();
            return createrOwner.Entity;
        }

        internal async Task UpdateOwner(Owner owner)
        {
            var dbOwner = await _dbContext.Owners.FindAsync(owner.ID);
            if (dbOwner == null)
            {
                throw new RepositoryException(RepositiryExceptionType.UserNotFound, owner.ID);
            }
            _dbContext.Entry(dbOwner).CurrentValues.SetValues(owner);
            await _dbContext.SaveChangesAsync();
        }


        internal async Task DeleteOwner(int ownerId)
        {
            //var gallery = await _dbContext.MediaContainers.FindAsync(userId);
            var owner = await _dbContext.Owners.FindAsync(ownerId);
            if (owner == null)
            {
                throw new RepositoryException(RepositiryExceptionType.UserNotFound, ownerId);
            }
            _dbContext.Owners.Remove(owner);
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
