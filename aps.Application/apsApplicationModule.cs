using System.Reflection;
using Abp.Modules;

namespace aps
{
    [DependsOn(typeof(apsCoreModule))]
    public class apsApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
