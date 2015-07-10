using System.Reflection;
using Abp.Modules;

namespace aps
{
    public class apsCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
