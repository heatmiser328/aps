using Abp.Web.Mvc.Controllers;

namespace aps.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class apsControllerBase : AbpController
    {
        protected apsControllerBase()
        {
            LocalizationSourceName = apsConsts.LocalizationSourceName;
        }
    }
}