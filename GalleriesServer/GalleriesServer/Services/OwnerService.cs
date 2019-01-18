using GalleriesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Services
{
    public class OwnerService
    {
        private readonly GalleriesDbContext _dbContext;
        public OwnerService(GalleriesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOwner(string emailAddress, string identityProvider, string userId, string firstName, string lastName)
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
    }
}
