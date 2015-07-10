using Abp.Application.Services;

namespace aps
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class apsAppServiceBase : ApplicationService
    {
        protected apsAppServiceBase()
        {
            LocalizationSourceName = apsConsts.LocalizationSourceName;
        }
    }
}