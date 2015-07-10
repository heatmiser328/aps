using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace aps
{
    [DependsOn(typeof(AbpWebApiModule), typeof(apsApplicationModule))]
    public class apsWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(apsApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
