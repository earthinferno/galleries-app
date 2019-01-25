using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Extensions;

namespace GalleriesServer.Models
{
    public enum RepositiryExceptionType
    {
        // Owner
        [Description("User account already exists.")]
        DuplicateUser,
        [Description("Email address already in use.")]
        DuplicateEmailAddress,

        // Gallery
        [Description("Media item not found for ID.")]
        MediaItemNotFoundForId,
        
    }

    public class RepositoryException : Exception
    {
        public RepositoryException(RepositiryExceptionType type)
        {
            ExceptionType = type;
        }

        public RepositoryException(RepositiryExceptionType type, string value) : base(type.ToDescriptionString(value))
        {
            ExceptionType = type;
        }

        public RepositoryException(RepositiryExceptionType type, int value) : base(type.ToDescriptionString(value.ToString()))
        {
            ExceptionType = type;
        }

        public RepositiryExceptionType ExceptionType { get; }
    }
}
