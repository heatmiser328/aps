using Abp.Web.Mvc.Views;

namespace aps.Web.Views
{
    public abstract class apsWebViewPageBase : apsWebViewPageBase<dynamic>
    {

    }

    public abstract class apsWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected apsWebViewPageBase()
        {
            LocalizationSourceName = apsConsts.LocalizationSourceName;
        }
    }
}