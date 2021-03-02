using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConsulClient.Models;

namespace ConsulClients.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static int sIndex = 0;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string server = "测试服务";
            string url = "http://address/api/WeatherForecast/Test";
            var consul = new Consul.ConsulClient(c =>
            {
                c.Address = new Uri("http://127.0.0.1:8500");
            });
            using (consul)
            {
                //取在Consul注册的全部服务
                var services = consul.Agent.Services().Result.Response.Select(s => s.Value)
                    .Where(s => s.Service == server).ToList();
                //轮询
                var ser = services[sIndex++ % services.Count];
                url = url.Replace("address", $"{ser.Address}:{ser.Port}");
            }
            ViewData["rsp"] = WebRequestHelper.GetHttp(url, "");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
