using LoveBank.Common;
namespace LoveBank.MVC.SiteMap
{
    public class SiteMapNodeFactory
    {
        private readonly SiteMapNode parent;

        public SiteMapNodeFactory(SiteMapNode parent)
        {
            Check.Argument.IsNotNull(parent, "parent");

            this.parent = parent;
        }

        public SiteMapNodeBuilder Add()
        {
            var node = new SiteMapNode();

            parent.ChildNodes.Add(node);

            return new SiteMapNodeBuilder(node);
        }
    }
}
