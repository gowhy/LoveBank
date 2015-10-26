using System.Web.Mvc;

namespace LoveBank.MVC.ModelBinder
{
    public class CustomModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.IsArray)
            {
                var key = bindingContext.ModelName + "[]";
                var valueResult = bindingContext.ValueProvider.GetValue(key);
                if (valueResult != null && !string.IsNullOrEmpty(valueResult.AttemptedValue))
                {
                    bindingContext.ModelName = key;
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
