namespace LoveBank.MVC.SiteMap
{
    public static class SiteMapManager
    {
        private static readonly SiteMapDictionary siteMaps = new SiteMapDictionary();

        public static SiteMapDictionary SiteMaps
        {
            get
            {
                return siteMaps;
            }
        }

        internal static void Clear()
        {
            SiteMaps.Clear();
        }
    }
}
