using System.Globalization;
using System.Resources;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class ResourceHelper
    {
        private static ResourceManager resourceManager = new ResourceManager("Infrastructure.Resources.Resources", typeof(ResourceHelper).Assembly);

        public static Task<string> GetStringAsync(string key)
        {
            return Task.FromResult(resourceManager.GetString(key, CultureInfo.CurrentCulture));
        }
    }
}
