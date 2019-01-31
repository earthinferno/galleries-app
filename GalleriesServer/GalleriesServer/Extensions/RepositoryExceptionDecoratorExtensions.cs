using GalleriesServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Extensions
{
    public static class RepositoryExceptionDecoratorExtensions
    {
        public static string ToDescriptionString(this RepositiryExceptionType val, string suppliedValue)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 
                ? attributes[0].Description + " Value: " + suppliedValue 
                : "Undefined exception type.  Value: " + suppliedValue;
        }

    }
}
