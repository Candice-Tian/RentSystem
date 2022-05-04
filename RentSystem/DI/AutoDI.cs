using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.DI
{
    public static class AutoDI
    {
        /// <summary>
        /// 注入数据
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            #region 依赖注入
            var transientType = typeof(IDependency); //每次新建
            var singletonType = typeof(IDependencySingleton); //全局唯一
            //获取实现了接口IDenpendency和IDenpendcySingleton的程序集
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(transientType) || t.GetInterfaces().Contains(singletonType)));
            //class的程序集
            var implementTypes = allTypes.Where(x => x.IsClass).ToArray();
            //接口的程序集
            var interfaceTypes = allTypes.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                //class有接口，用接口注入
                if (interfaceType != null)
                {
                    //判断用什么方式注入
                    if (interfaceType.GetInterfaces().Contains(transientType))
                    {
                        services.AddScoped(interfaceType, implementType);
                    }
                    else if (interfaceType.GetInterfaces().Contains(singletonType))
                    {
                        services.AddSingleton(interfaceType, implementType);
                    }
                }
                else //class没有接口，直接注入class
                {
                    //判断用什么方式注入
                    if (implementType.GetInterfaces().Contains(transientType))
                    {
                        services.AddTransient(implementType);
                    }
                    else if (implementType.GetInterfaces().Contains(singletonType))
                    {
                        services.AddSingleton(implementType);
                    }
                }
            }
            #endregion
            return services;
        }
    }
}
