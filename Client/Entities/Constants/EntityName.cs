using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Client.Entities.Constants
{
    public static class EntityName
    {
        public static readonly List<string> NameList = GetEntityNames();
        
        private static List<string> GetEntityNames()
        {
            var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("Client.Entities"))
                .Select(t => t.Name)
                .ToList();

            return entityTypes;
        }
    }
}