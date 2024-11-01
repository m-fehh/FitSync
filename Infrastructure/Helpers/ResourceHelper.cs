using System.Globalization;
using System.Resources;

namespace Infrastructure.Helpers
{
    public static class ResourceHelper
    {
        private static ResourceManager resourceManager = new ResourceManager("Infrastructure.Resources.Resources", typeof(ResourceHelper).Assembly);

        public static Task<string> GetStringAsync(string key)
        {
            var result = resourceManager.GetString(key, CultureInfo.CurrentCulture);
            return Task.FromResult(result ?? key);
        }
    }
}
