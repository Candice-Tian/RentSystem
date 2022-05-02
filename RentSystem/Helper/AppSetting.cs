using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RentSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Helper
{
    public static class AppSetting
    {
        private static IConfigurationSection _configuratuinSection = null;

        /// <summary>
        /// 根据KEY获取appsettings.json文件中配置的信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return _configuratuinSection.GetSection(key)?.Value;
        }

        /// <summary>
        /// 获取数据库连接地址
        /// </summary>
        /// <returns></returns>
        public static Connections GetConnections()
        {
            string appSettings = _configuratuinSection.GetSection("Connections").Value;
            return JsonConvert.DeserializeObject<Connections>(appSettings);
        }

    }
}
