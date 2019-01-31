using System;

namespace GalleriesServer.Services
{
    public static class Guard
    {
        public static void GuardStringValue(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("name");
        }
    }
}
