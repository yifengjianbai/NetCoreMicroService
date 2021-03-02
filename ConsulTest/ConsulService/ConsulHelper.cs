using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;

namespace ConsulService
{
    public static class ConsulHelper
    {
        public static void RegistServer(this IConfiguration configuration)
        {
            ConsulClient consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500");
                c.Datacenter = "dc";
            });

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);

            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"{ip}:{port}",
                Name = "TestService",
                Address = ip,
                Port = port,
                //健康检查
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://{ip}:{port}/api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(12)
                }
            });
        }
    }
}
