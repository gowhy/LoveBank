using System;
using System.Web.Mvc;
using LoveBank.Common;

namespace LoveBank.MVC.UI {
    /// <summary>
    /// Extension methods for the pager.
    /// </summary>
    public static class PaginationExtensions {
        /// <summary>
        /// Creates a pager component using the item from the viewdata with the specified key as the datasource.
        /// </summary>
        /// <param name="helper">The HTML Helper</param>
        /// <param name="viewDataKey">The viewdata key</param>
        /// <returns>A Pager component</returns>
        public static Pager Pager(this HtmlHelper helper, string viewDataKey) {
            var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IPagedList;

            if (dataSource == null) {
                throw new InvalidOperationException(
                    string.Format("Item in ViewData with key '{0}' is not an IPagination.",
                                  viewDataKey));
            }

            return helper.Pager(dataSource);
        }

        /// <summary>
        /// Creates a pager component using the specified IPagination as the datasource.
        /// </summary>
        /// <param name="helper">The HTML Helper</param>
        /// <param name="pagination">The datasource</param>
        /// <returns>A Pager component</returns>
        public static Pager Pager(this HtmlHelper helper, IPagedList pagination) {
            return new Pager(pagination, helper.ViewContext);
        }

        public static ClientPager ClientPager(this HtmlHelper helper, IPagedList pagination) {
            return new ClientPager(pagination, helper.ViewContext);
        }
    }
}