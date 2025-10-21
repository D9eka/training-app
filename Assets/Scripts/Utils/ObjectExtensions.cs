using System;

namespace Utils
{
    public static class ObjectExtensions
    {
        public static string GetId(this object parameter, bool needNonEmpty = true)
        {
            if (parameter is string id)
            {
                if (needNonEmpty && string.IsNullOrEmpty(id))
                    throw new ArgumentException("Requires a non-empty string ID", nameof(parameter));
                return id;
            }
            return !needNonEmpty ? String.Empty : throw new ArgumentException("Parameter must be a string", nameof(parameter));
        }
    }
}