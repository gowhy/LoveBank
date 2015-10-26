using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace LoveBank.Common.Unity
{
    public class PreApplicationStartCode
    {
        private static bool _isStarting;

        public static void PreStart()
        {
            if (!_isStarting)
            {
                _isStarting = true;
                DynamicModuleUtility.RegisterModule(typeof(PerRequestLifetimeModule));
            }
        }
    }
}
