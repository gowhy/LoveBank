namespace LoveBank.Lang
{

    public static class L
    {
        private static Common _common;

        private static readonly object SyncRoot = new object();

        private static Common GetInstance()
        {

            if (_common == null)
            {
                lock (SyncRoot)
                {
                    if (_common == null)
                    {
                        _common = new Common();
                    }
                }
            }
            return _common;

        }

        /// <summary>
        /// 返回通用的语言集
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Common(string key)
        {
            return !GetInstance().ContainsKey(key) ? "" : GetInstance()[key];
        }
    }
}
